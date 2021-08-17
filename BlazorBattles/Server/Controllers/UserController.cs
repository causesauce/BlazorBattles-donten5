using BlazorBattles.Server.Data;
using BlazorBattles.Server.Services;
using BlazorBattles.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorBattles.Server.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUtilityService _utilityService;

        public UserController(DataContext context, IUtilityService utilityService)
        {
            _context = context;
            _utilityService = utilityService;
        }

        [HttpGet("getbananas")]
        public async Task<IActionResult> GetBananas()
        {
            var user = await _utilityService.GetUser();
            return Ok(user.Bananas);
        }

        [HttpPut("addbananas")]
        public async Task<IActionResult> AddBananas([FromBody] int bananas)
        {
            var user = await _utilityService.GetUser();
            user.Bananas += bananas;

            await _context.SaveChangesAsync();
            return Ok(user.Bananas);
        }

        [HttpGet("leaderboard")]
        public async Task<IActionResult> GetLeaderboard()
        {
            var users = await _context.Users.Where(user => !user.IsDeleted && user.IsConfirmed).ToListAsync();

            users = users
                .OrderByDescending(u => u.Victories)
                .ThenBy(u => u.Defeats)
                .ThenBy(u => u.DateCreated)
                .ToList();

            int rank = 1;
            var response = users.Select(
                user => new UserStatistic
                {
                    Rank = rank++,
                    UserId = user.Id,
                    Username = user.Username,
                    Battles = user.Battles,
                    Victories = user.Victories,
                    Defeats = user.Defeats
                });

            return Ok(response);

        }
        /*
                private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                private async Task<User> GetUser() => await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            */

        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var user = await _utilityService.GetUser();
            var battles = await _context.Battles
                .Where(b => b.AttackerId == user.Id || b.OpponentId == user.Id)
                .Include(b => b.Attacker)
                .Include(b => b.Opponent)
                .Include(b => b.Winner)
                .ToListAsync();

            var history = battles.Select(b => new BattleHistoryEntry
            {
                BattleId = b.Id,
                AttackerId = b.AttackerId,
                OpponentId = b.OpponentId,
                YouWon = b.WinnerId == user.Id,
                AttackerName = b.Attacker.Username,
                OpponentName = b.Opponent.Username,
                RoundsFought = b.RoundsFought,
                WinnerDamageDealt = b.WinnerDamage,
                BattleDate = b.BattleDate
            });

            return Ok(history.OrderByDescending(h => h.BattleDate));
        }

    }


}
