using ExemploAPI.Models.Request;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ExemploAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProdutoController : ControllerBase
	{
		private readonly string _produtoCaminhoArquivo;

		public ProdutoController()
		{
			_produtoCaminhoArquivo = Path.Combine(Directory.GetCurrentDirectory(), "Data", "produto.json");
		}

		#region Métodos arquivo
		private List<ProdutoViewModel> LerProdutosDoArquivo()
		{
			if (!System.IO.File.Exists(_produtoCaminhoArquivo))
			{
				return new List<ProdutoViewModel>();
			}

			string json = System.IO.File.ReadAllText(_produtoCaminhoArquivo);
			return JsonConvert.DeserializeObject<List<ProdutoViewModel>>(json);
		}

		private int ObterProximoCodigoDisponivel()
		{
			List<ProdutoViewModel> produtos = LerProdutosDoArquivo();
			if (produtos.Any())
			{
				return produtos.Max(p => p.Codigo) + 1;
			}
			else
			{
				return 1;
			}
		}

		private void EscreverProdutosNoArquivo(List<ProdutoViewModel> produtos)
		{
			string json = JsonConvert.SerializeObject(produtos);
			System.IO.File.WriteAllText(_produtoCaminhoArquivo, json);
		}
		#endregion

		#region Operações CRUD

		[HttpGet]
		public IActionResult Get()
		{
			List<ProdutoViewModel> produtos = LerProdutosDoArquivo();
			return Ok(produtos);
		}

		[HttpGet("{codigo}")]
		public IActionResult Get(int codigo)
		{
			List<ProdutoViewModel> produtos = LerProdutosDoArquivo();
			ProdutoViewModel produto = produtos.Find(p => p.Codigo == codigo);
			if (produto == null)
			{
				return NotFound();
			}

			return Ok(produto);
		}

		[HttpPost]
		public IActionResult Post([FromBody] NovoProdutoViewModel produto)
		{
			if (produto == null)
			{
				return BadRequest();
			}

			List<ProdutoViewModel> produtos = LerProdutosDoArquivo();
			int proximoCodigo = ObterProximoCodigoDisponivel();

			ProdutoViewModel novoProduto = new ProdutoViewModel()
			{
				Codigo = proximoCodigo,
				Descricao = produto.Descricao,
				Preco = produto.Preco,
				Estoque = produto.Estoque
			};

			produtos.Add(novoProduto);
			EscreverProdutosNoArquivo(produtos);

			return CreatedAtAction(nameof(Get), new { codigo = novoProduto.Codigo }, novoProduto);
		}

		[HttpPut("{codigo}")]
		public IActionResult Put(int codigo, [FromBody] EditaProdutoViewModel produto)
		{
			if (produto == null )
				return BadRequest();

			List<ProdutoViewModel> produtos = LerProdutosDoArquivo();
			int index = produtos.FindIndex(p => p.Codigo == codigo);
			if (index == -1)
				return NotFound();

			ProdutoViewModel produtoEditado = new ProdutoViewModel()
			{
				Codigo = codigo,
				Estoque = produto.Estoque,
				Descricao = produto.Descricao,
				Preco = produto.Preco
			};

			produtos[index] = produtoEditado;
			EscreverProdutosNoArquivo(produtos);

			return NoContent();
		}

		[HttpDelete("{codigo}")]
		public IActionResult Delete(int codigo)
		{
			List<ProdutoViewModel> produtos = LerProdutosDoArquivo();
			ProdutoViewModel produto = produtos.Find(p => p.Codigo == codigo);
			if (produto == null)
				return NotFound();

			produtos.Remove(produto);
			EscreverProdutosNoArquivo(produtos);

			return NoContent();
		}
		#endregion
	}
}