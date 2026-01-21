using BE_Glowpurea.Dtos.Address;

namespace BE_Glowpurea.IServices
{
    public interface IAddressService
    {
        Task<List<AddressResponse>> GetMyAddressesAsync(int accountId);
        Task<int> CreateAsync(int accountId, CreateAddressRequest request);
        Task UpdateAsync(int addressId, int accountId, UpdateAddressRequest request);
        Task DeleteAsync(int addressId, int accountId);
        Task SetDefaultAsync(int addressId, int accountId);
    }
}
