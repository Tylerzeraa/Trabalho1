using ExemploAPI.Models;
using ExemploAPI.Models.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;
using ExemploAPI;

namespace ExemploAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlunoController : ControllerBase
    {
        private static List<AlunoViewModel> _alunos = new List<AlunoViewModel>();
        private static int _nextId = 1;

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_alunos);
        }
    }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var aluno = _alunos.FirstOrDefault(a => a.Id == id);
            if (aluno == null)
                return NotFound();

            return Ok(aluno);
        }

        [HttpPost]
        public IActionResult Post(AlunoViewModel alunoViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            alunoViewModel.Id = _nextId++;
            _alunos.Add(alunoViewModel);

            return Ok(new NovoAlunoCriadoResponse { Sucesso = true, Mensagem = "Aluno criado com sucesso!" });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, AlunoViewModel alunoViewModel)
        {
            var alunoExistente = _alunos.FirstOrDefault(a => a.Id == id);
            if (alunoExistente == null)
                return NotFound();

            alunoViewModel.Id = id;
            _alunos.Remove(alunoExistente);
            _alunos.Add(alunoViewModel);

            return Ok(new NovoAlunoCriadoResponse { Sucesso = true, Mensagem = "Aluno atualizado com sucesso!" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var alunoExistente = _alunos.FirstOrDefault(a => a.Id == id);
            if (alunoExistente == null)
                return NotFound();

            _alunos.Remove(alunoExistente);

            return Ok(new NovoAlunoCriadoResponse { Sucesso = true, Mensagem = "Aluno removido com sucesso!" });
        }
    }
}
