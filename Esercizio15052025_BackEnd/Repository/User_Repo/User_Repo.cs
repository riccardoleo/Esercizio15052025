using AutoMapper;
using Esercizio15052025.Models;
using Esercizio20052025.DTO.Users_DTO;
using Esercizio20052025.Repository.User_Repo.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Esercizio20052025.Repository.User_Repo
{
    public class User_Repo : IUser_Repo
    {
        private readonly EQUIPPINGContext _context;
        private readonly IMapper _mapper;
        public User_Repo(EQUIPPINGContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(User entity)
        {
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(User entity)
        {
            _context.Users.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public bool ExistsByNameBool(string Username)
        {
            return _context.Users.Any(t => t.Username == Username);
        }

        public User? ReturnUserByName(string Uname)
        {
            return _context.Users.FirstOrDefault(t => t.Username == Uname);
        }

        public User? ReturnUserDtoByName(string name)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == name);
            return user == null ? null : _mapper.Map<User>(user);
        }
    }
}
