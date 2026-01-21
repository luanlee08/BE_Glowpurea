using BE_Glowpurea.Dtos.Address;
using BE_Glowpurea.IServices;
using BE_Glowpurea.Models;
using Microsoft.EntityFrameworkCore;

namespace BE_Glowpurea.Services
{
    public class AddressService : IAddressService
    {
        private readonly DbGlowpureaContext _context;

        public AddressService(DbGlowpureaContext context)
        {
            _context = context;
        }

        public async Task<List<AddressResponse>> GetMyAddressesAsync(int accountId)
        {
            return await _context.Addresses
                .Where(x => x.AccountID == accountId && !x.IsDeleted)
                .OrderByDescending(x => x.IsDefault)
                .Select(x => new AddressResponse
                {
                    AddressID = x.AddressID,
                    AddressLine = x.AddressLine,
                    City = x.City,
                    Ward = x.Ward,
                    IsDefault = x.IsDefault
                })
                .ToListAsync();
        }

        public async Task<int> CreateAsync(int accountId, CreateAddressRequest request)
        {
            var hasAnyAddress = await _context.Addresses
       .AnyAsync(x => x.AccountID == accountId && !x.IsDeleted);

            // Nếu là địa chỉ đầu tiên → auto default
            var isDefault = !hasAnyAddress || request.IsDefault;

            if (isDefault)
            {
                await _context.Addresses
                    .Where(x => x.AccountID == accountId && !x.IsDeleted)
                    .ExecuteUpdateAsync(s => s.SetProperty(p => p.IsDefault, false));
            }

            var address = new Address
            {
                AccountID = accountId,
                AddressLine = request.AddressLine,
                City = request.City,
                Ward = request.Ward,
                IsDefault = isDefault,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            return address.AddressID;
        }

        public async Task UpdateAsync(int addressId, int accountId, UpdateAddressRequest request)
        {
            var address = await _context.Addresses
                .FirstOrDefaultAsync(x => x.AddressID == addressId && x.AccountID == accountId && !x.IsDeleted);

            if (address == null)
                throw new Exception("Địa chỉ không tồn tại");

            if (request.IsDefault == true)
            {
                await _context.Addresses
                    .Where(x => x.AccountID == accountId && !x.IsDeleted)
                    .ExecuteUpdateAsync(s => s.SetProperty(p => p.IsDefault, false));

                address.IsDefault = true;
            }
            // ❌ KHÔNG set gì nếu IsDefault == null

            address.AddressLine = request.AddressLine;
            address.City = request.City;
            address.Ward = request.Ward;
            address.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int addressId, int accountId)
        {
            var address = await _context.Addresses
      .FirstOrDefaultAsync(x =>
          x.AddressID == addressId &&
          x.AccountID == accountId &&
          !x.IsDeleted);

            if (address == null)
                throw new Exception("Địa chỉ không tồn tại");

            var wasDefault = address.IsDefault;

            address.IsDeleted = true;
            address.IsDefault = false;
            address.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Nếu xóa địa chỉ mặc định → set cái khác làm default
            if (wasDefault)
            {
                var nextAddress = await _context.Addresses
                    .Where(x =>
                        x.AccountID == accountId &&
                        !x.IsDeleted)
                    .OrderBy(x => x.AddressID)
                    .FirstOrDefaultAsync();

                if (nextAddress != null)
                {
                    nextAddress.IsDefault = true;
                    nextAddress.UpdatedAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task SetDefaultAsync(int addressId, int accountId)
        {
            var address = await _context.Addresses
                .FirstOrDefaultAsync(x => x.AddressID == addressId && x.AccountID == accountId && !x.IsDeleted);

            if (address == null)
                throw new Exception("Địa chỉ không tồn tại");

            await _context.Addresses
                .Where(x => x.AccountID == accountId)
                .ExecuteUpdateAsync(s => s.SetProperty(p => p.IsDefault, false));

            address.IsDefault = true;
            address.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}
