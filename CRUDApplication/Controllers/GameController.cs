using AutoMapper;
using CRUDApplication.Domain.Contracts;
using CRUDApplication.Domain.Entities;
using CRUDApplication.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CRUDApplication.Controllers
{
    [ApiController]
    [Route("api/")]
    public class GameController : ControllerBase
    {
        private readonly IBaseRepository<Game> _context;
        private readonly IMapper _mapper;

        public GameController(IBaseRepository<Game> context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("game")]
        public async Task<IActionResult> GetGames()
        {
            var games = await _context.GetAll(record => record.Company);
            var gamesDto = _mapper.Map<List<GameDto>>(games);
            return Ok(gamesDto);
        }

        [HttpGet("game/{id}")]
        public async Task<IActionResult> GetGame(Guid id)
        {
            var game = await _context.Get(id);
            var gameDto = _mapper.Map<GameDto>(game);
            return Ok(gameDto);
        }

        [HttpPost("game")]
        public async Task<IActionResult> CreateGame([FromBody] CreateGame createGame)
        {
            var game = _mapper.Map<Game>(createGame);
            var createdGame= await _context.Add(game);
            var gameDto = _mapper.Map<GameDto>(createdGame);
            return CreatedAtAction(nameof(GetGame), new { id = gameDto.Id }, gameDto);
        }

        [HttpPut("game")]
        public async Task<IActionResult> UpdateGame(Guid id, [FromBody] UpdateGame updateGame)
        {
            var game = await _context.Get(id);
            if (game == null)
                return NotFound();
            _mapper.Map(updateGame, game);
            var updatedGame = await _context.Update(game);
            var gameDto = _mapper.Map<GameDto>(updatedGame);
            return Ok(gameDto);
        }

        [HttpDelete("game")]
        public async Task<IActionResult> DeleteGame(Guid id)
        {
            var game = await _context.Delete(id);
            if (game == null)
                return NotFound();
            var gameDto = _mapper.Map<GameDto>(game);
            return Ok(gameDto);
        }
    }
}
