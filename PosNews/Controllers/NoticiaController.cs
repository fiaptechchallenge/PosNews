using AutoMapper;
using Infraestrutura.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosNews.Interfaces;
using PosNews.Models.Dto;

namespace PosNews.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NoticiaController : ControllerBase
    {
        private readonly INoticiaRepository _noticiaRepository;
        private readonly IMapper _mapper;

        public NoticiaController(INoticiaRepository noticiaRepository,
                                 IMapper mapper)
        {
            _noticiaRepository = noticiaRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// API para buscar uma noticia atraves do ID informado.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<NoticiaDto>> GetNoticiaById(int id)
        {
            Noticia noticia = await _noticiaRepository.GetAsync(x => x.Id == id);
            if (noticia == null) return NotFound();

            return Ok(noticia);
        }

        /// <summary>
        /// API para buscar todas as noticias
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<NoticiaDto>>> GetAllAsync()
        {
            IEnumerable<Noticia> noticia = await _noticiaRepository.GetAllAsync();
            return Ok(noticia);
        }

        /// <summary>
        /// API para criar uma nova noticia.
        /// </summary>
        /// <param name="noticia"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Noticia>> CreateNoticiaAsync([FromBody] NoticiaDto noticia)
        {
            if (noticia == null) return BadRequest(noticia);
            Noticia modelNoticia = _mapper.Map<Noticia>(noticia);

            await _noticiaRepository.CreateAsync(modelNoticia);
            return CreatedAtAction(nameof(GetNoticiaById), new { id = modelNoticia.Id }, noticia);
        }
    }
}
