using BE_Glowpurea.Dtos.Auth;
using BE_Glowpurea.Helpers;
using BE_Glowpurea.IRepositories;
using BE_Glowpurea.IServices;
using BE_Glowpurea.Models;

namespace BE_Glowpurea.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAccountRepository _accountRepo;
        private readonly IRoleRepository _roleRepo;
        private readonly IEmailOtpRepository _otpRepo;
        private readonly IEmailService _emailService;
        private readonly JwtHelper _jwtHelper;

        public AuthService(
            IAccountRepository accountRepo,
            IRoleRepository roleRepo,
            IEmailOtpRepository otpRepo,
            IEmailService emailService,
            JwtHelper jwtHelper)
        {
            _accountRepo = accountRepo;
            _roleRepo = roleRepo;
            _otpRepo = otpRepo;
            _emailService = emailService;
            _jwtHelper = jwtHelper;
        }

        // ================= REGISTER =================
        public async Task RegisterAsync(RegisterRequest request)
        {
            var existingAccount = await _accountRepo.GetByEmailAsync(request.Email);

            // ===== EMAIL ĐÃ TỒN TẠI =====
            if (existingAccount != null)
            {
                // 👉 ĐÃ ĐĂNG KÝ NHƯNG CHƯA VERIFY → RESEND OTP
                if (existingAccount.Status == "Pending")
                {
                    await ResendOtpAsync(existingAccount);
                    return;
                }

                throw new Exception("Email đã tồn tại");
            }

            // ===== CHECK PHONE =====
            if (!string.IsNullOrEmpty(request.PhoneNumber))
            {
                if (await _accountRepo.PhoneExistsAsync(request.PhoneNumber))
                    throw new Exception("Số điện thoại đã tồn tại");
            }

            // ===== CREATE ACCOUNT =====
            var role = await _roleRepo.GetCustomerRoleAsync();

            var account = new Account
            {
                AccountName = request.AccountName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                RoleId = role.RoleId,
                Status = "Pending",
                CreatedAt = DateTime.Now
            };

            await _accountRepo.CreateAsync(account);

            // ===== SEND OTP =====
            await CreateAndSendOtpAsync(account);
        }

        // ================= VERIFY OTP =================
        public async Task VerifyOtpAsync(VerifyOtpRequest request)
        {
            var otp = await _otpRepo.GetValidOtpAsync(
                request.Email,
                request.OtpCode,
                "Register"
            );

            if (otp == null)
                throw new Exception("OTP không hợp lệ hoặc đã hết hạn");

            var account = await _accountRepo.GetByEmailAsync(request.Email);
            if (account == null)
                throw new Exception("Account không tồn tại");

            account.Status = "Active";

            await _accountRepo.UpdateAsync(account);
            await _otpRepo.MarkUsedAsync(otp);
        }

        // ================= LOGIN =================
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            Account? account;

            if (request.Identifier.Contains("@"))
                account = await _accountRepo.GetByEmailAsync(request.Identifier);
            else
                account = await _accountRepo.GetByPhoneAsync(request.Identifier);

            if (account == null)
                throw new Exception("Tài khoản không tồn tại");

            if (!BCrypt.Net.BCrypt.Verify(request.Password, account.Password))
                throw new Exception("Sai mật khẩu");

            if (account.Status == "Pending")
                throw new Exception("Tài khoản chưa xác thực email");

            if (account.Status == "Blocked")
                throw new Exception("Tài khoản bị khóa");

            var token = _jwtHelper.GenerateToken(
                account,
                account.Role!.RoleName
            );

            return new LoginResponse
            {
                Token = token,
                Email = account.Email,
                Role = account.Role.RoleName
            };
        }

        // ================= PRIVATE METHODS =================

        private async Task CreateAndSendOtpAsync(Account account)
        {
            var otpCode = new Random().Next(100000, 999999).ToString();

            var otp = new EmailOtp
            {
                AccountID = account.AccountId,
                Email = account.Email,
                Purpose = "Register",
                OtpCode = otpCode,
                CreatedAt = DateTime.Now,
                ExpiresAt = DateTime.Now.AddMinutes(5),
                IsUsed = false
            };

            await _otpRepo.CreateAsync(otp);

            var emailBody = BuildOtpEmail(account.AccountName, otpCode);

            await _emailService.SendAsync(
                account.Email,
                "Xác thực đăng ký Glowpurea 🌿",
                emailBody
            );

        }

        private async Task ResendOtpAsync(Account account)
        {
            await _otpRepo.DeleteByAccountIdAsync(account.AccountId);
            await CreateAndSendOtpAsync(account);
        }

        private string BuildOtpEmail(string name, string otp)
        {                        return $@"
            <!DOCTYPE html>
            <html>
            <head>
              <meta charset='UTF-8' />
              <title>Glowpurea OTP</title>
            </head>
            <body style='margin:0;padding:0;background-color:#f4f6f8;'>

              <table width='100%' cellpadding='0' cellspacing='0' style='background-color:#f4f6f8;'>
                <tr>
                  <td align='center' style='padding:30px 0;'>

                    <table width='600' cellpadding='0' cellspacing='0'
                           style='background:#ffffff;border-radius:14px;overflow:hidden;
                                  font-family:Arial,sans-serif;box-shadow:0 8px 30px rgba(0,0,0,0.08);'>

                      <!-- HERO / BANNER -->
                      <tr>
                        <td style='position:relative;'>
                          <img src='https://file.hstatic.net/200000073977/file/son-duong-thien-nhien-2_c18531544fff447dabff669821328b6f.png'
                               width='600'
                               style='display:block;' />
                        </td>
                      </tr>

                      <!-- CONTENT -->
                      <tr>
                        <td style='padding:35px 40px;'>

                          <h2 style='color:#2f855a;margin:0 0 15px 0;font-size:26px;'>
                            🌿 Chào mừng đến với Glowpurea
                          </h2>

                          <p style='font-size:15px;color:#555;line-height:1.7;margin-bottom:14px;'>
                            Glowpurea là nơi <strong>đôi môi được nâng niu mỗi ngày</strong> với
                            những dòng <strong>son dưỡng thuần thiên nhiên</strong>.
                            Chúng tôi tin rằng vẻ đẹp thật sự đến từ sự tinh khiết,
                            an toàn và yêu thương bản thân.
                          </p>

                          <p style='font-size:15px;color:#555;line-height:1.7;'>
                            Từ <strong>sáp ong</strong>, <strong>dầu thực vật</strong> đến
                            các thành phần lành tính được chọn lọc kỹ lưỡng,
                            Glowpurea mang đến cho bạn
                            <strong>đôi môi mềm mại – khỏe mạnh – rạng rỡ</strong>.
                          </p>

                          <hr style='border:none;border-top:1px solid #eee;margin:30px 0;' />

                          <p style='font-size:15px;color:#555;margin-bottom:6px;'>
                            Xin chào <strong>{name}</strong>,
                          </p>

                          <p style='font-size:15px;color:#555;margin-top:0;'>
                            Vui lòng nhập mã OTP bên dưới để hoàn tất đăng ký:
                          </p>

                          <!-- OTP BOX -->
                          <div style='
                            margin:30px auto;
                            text-align:center;
                            font-size:34px;
                            letter-spacing:8px;
                            font-weight:bold;
                            color:#2f855a;
                            background:linear-gradient(135deg,#edfdf6,#d1fae5);
                            padding:18px 20px;
                            border-radius:12px;
                            width:fit-content;
                          '>
                            {otp}
                          </div>

                          <p style='font-size:14px;color:#777;margin-bottom:6px;'>
                            ⏰ Mã OTP có hiệu lực trong <strong>5 phút</strong>
                          </p>

                          <p style='font-size:13px;color:#999;'>
                            Nếu bạn không thực hiện đăng ký Glowpurea,
                            vui lòng bỏ qua email này.
                          </p>

                        </td>
                      </tr>

                      <!-- FOOTER -->
                      <tr>
                        <td style='background:#f9fafb;padding:25px;text-align:center;'>

                          <p style='font-size:13px;color:#777;margin:0;'>
                            🌱 Glowpurea — Natural Lip Care
                          </p>

                          <p style='font-size:12px;color:#aaa;margin:6px 0 0 0;'>
                            Chăm sóc đôi môi bạn bằng sự tinh khiết của thiên nhiên
                          </p>

                          <p style='font-size:11px;color:#bbb;margin-top:10px;'>
                            © {DateTime.Now.Year} Glowpurea. All rights reserved.
                          </p>

                        </td>
                      </tr>

                    </table>

                  </td>
                </tr>
              </table>

            </body>
            </html>
            ";

        }

        public async Task<ProfileResponse> GetProfileAsync(string email)
        {
            var account = await _accountRepo.GetByEmailAsync(email);
            if (account == null)
                throw new Exception("Không tìm thấy tài khoản");

            return new ProfileResponse
            {
                AccountId = account.AccountId,
                AccountName = account.AccountName,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber,
                Role = account.Role!.RoleName,
                Image = account.Image
            };
        }

        public async Task<string> UploadAvatarAsync(string email, IFormFile image)
        {
            var account = await _accountRepo.GetByEmailAsync(email);
            if (account == null)
                throw new Exception("Không tìm thấy tài khoản");

            if (!image.ContentType.StartsWith("image/"))
                throw new Exception("File không hợp lệ");

            if (image.Length > 2 * 1024 * 1024)
                throw new Exception("Ảnh tối đa 2MB");

            var folder = Path.Combine("wwwroot", "avatars");
            Directory.CreateDirectory(folder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var filePath = Path.Combine(folder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await image.CopyToAsync(stream);

            account.Image = $"/avatars/{fileName}";
            await _accountRepo.UpdateAsync(account);

            return account.Image;
        }
        public async Task<ProfileResponse> UpdateProfileAsync(
            string currentEmail,
            UpdateProfileRequest request)
        {
            var account = await _accountRepo.GetByEmailAsync(currentEmail);
            if (account == null)
                throw new Exception("Không tìm thấy tài khoản");

            // ✅ CHECK EMAIL MỚI
            if (!string.Equals(account.Email, request.Email, StringComparison.OrdinalIgnoreCase))
            {
                var existedEmail = await _accountRepo.GetByEmailAsync(request.Email);
                if (existedEmail != null)
                    throw new Exception("Email đã được sử dụng");
            }

            // ✅ CHECK PHONE
            if (!string.IsNullOrEmpty(request.PhoneNumber))
            {
                var existedPhone = await _accountRepo.GetByPhoneAsync(request.PhoneNumber);
                if (existedPhone != null && existedPhone.AccountId != account.AccountId)
                    throw new Exception("Số điện thoại đã được sử dụng");
            }

            account.AccountName = request.AccountName;
            account.PhoneNumber = request.PhoneNumber;
            account.Email = request.Email;
            account.UpdatedAt = DateTime.Now;

            await _accountRepo.UpdateAsync(account);

            return new ProfileResponse
            {
                AccountId = account.AccountId,
                AccountName = account.AccountName,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber,
                Role = account.Role!.RoleName,
                Image = account.Image
            };
        }

    }
}
