using ExemploAPI.Models.Request;
using ExemploAPI.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace ExemploAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class TesteController : ControllerBase
	{
		[HttpGet]
		public IActionResult Get()
		{
			return Ok("Olá mundo, minha primeira requisição GET");
		}

		[HttpGet("ObterPorId/{id}")]
		public IActionResult Get(int id)
		{
			return Ok($"Olá mundo,exemplo rota com id = {id}");
				
		}

		[HttpPost]
		public IActionResult Post(TesteViewModel testeViewModel)
		{
			if (testeViewModel.Idade < 18)
				return BadRequest(
					new NovoTesteCriadoResponse() 
					{ 
						sucesso = false, 
						mensagem = "não possível enviar idade menor que 18 anos" 
					}
				);
			return Ok(
				new NovoTesteCriadoResponse() 
				{ 
					sucesso = true, 
					mensagem = "registro criado com sucesso!" }
				);
		}
	}
}
