using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TesteTecnicoMaxiprod.WebApi.Data;
using TesteTecnicoMaxiprod.WebApi.DTOs;
using TesteTecnicoMaxiprod.WebApi.Models;

namespace TesteTecnicoMaxiprod.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController: ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todas as categorias cadastradas
        /// </summary>
        /// <returns>Lista de Categorias</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategorias()
        {
            var categorias = await _context.Categorias
                .Select(c => new CategoriaDTO
                {
                    Id = c.Id,
                    Descricao = c.Descricao,
                    Finalidade = c.Finalidade
                })
                .ToListAsync();

            return Ok(categorias);
        }

        /// <summary>
        /// Obtém categorias filtradas por finalidade
        /// Obs: Inclui as categorias "Ambas"
        /// </summary>
        /// <param name="finalidade">Finalidade filtro</param>
        /// <returns>Lista de Categorias</returns>
        [HttpGet("por-finalidade/{finalidade}")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasPorFinalidade(FinalidadeCategoria finalidade)
        {
            var categorias = await _context.Categorias
                .Where(c => c.Finalidade == finalidade || c.Finalidade == FinalidadeCategoria.Ambas)
                .Select(c => new CategoriaDTO
                {
                    Id = c.Id,
                    Descricao = c.Descricao,
                    Finalidade = c.Finalidade
                })
                .ToListAsync();

            return Ok(categorias);
        }
    }
}
