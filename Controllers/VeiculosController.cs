using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Veiculos.Api.Data;
using Veiculos.Api.Entity;

namespace Veiculos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VeiculosController : ControllerBase
    {
        public readonly ApplicationContext _applicationContext;
        public VeiculosController(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }



        //GET 
        [HttpGet("Cor/{cor}")]
        public async Task<ActionResult<IEnumerable<VeiculosEntity>>> GetVeiculosPorCor(string cor)
        {
            var veiculos = await _applicationContext.Veiculos
                .Where(v => v.Cor.ToUpper() == cor.ToUpper())
                .ToListAsync();

            if (veiculos == null || veiculos.Count == 0)
            {
                return NotFound($"Não foram encontrados veículos da cor {cor}.");
            }

            return veiculos;
        }


        //GET
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVeiculo(int id)
        {
            // Busca o veículo no banco de dados pelo ID
            var veiculo = await _applicationContext.Veiculos.FindAsync(id);

            // Se não encontrar, retorna NotFound
            if (veiculo == null)
            {
                return NotFound();
            }

            // Se encontrar, retorna o veículo
            return Ok(veiculo);
        }

        // DELETE: api/Veiculos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVeiculo(int id)
        {
            var veiculo = await _applicationContext.Veiculos.FindAsync(id);

            if (veiculo == null)
            {
                return NotFound();
            }

            _applicationContext.Veiculos.Remove(veiculo);
            await _applicationContext.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Veiculos
        [HttpPost]
        public async Task<IActionResult> PostVeiculo([FromBody] VeiculosEntity veiculo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _applicationContext.Veiculos.Add(veiculo);
                    await _applicationContext.SaveChangesAsync();

                    return CreatedAtAction("GetVeiculo", new { id = veiculo.Id }, veiculo);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                // Log do erro (use um mecanismo de logging apropriado, como Serilog ou ILogger)
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

                return StatusCode(500, "Erro interno no servidor ao processar a solicitação.");
            }
        }
    }
}