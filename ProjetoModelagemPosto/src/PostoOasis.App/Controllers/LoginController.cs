using PostoOasis.Core.Interfaces;
using PostoOasis.Core.Models;

namespace PostoOasis.App.Controllers;

public class LoginController
{
    private readonly IPostoRepository _repository;

    public LoginController(IPostoRepository repository)
    {
        _repository = repository;
    }

    public Funcionario? Autenticar(string usuario, string senha)
    {
        return _repository.ObterFuncionarioPorUsuario(usuario, senha);
    }

    public Funcionario? ObterPorPerfil(string perfil)
    {
        var funcs = _repository.ObterFuncionarios();
        return perfil.ToLowerInvariant() switch
        {
            "gerente" => funcs.FirstOrDefault(f => f.Cargo == "Gerente"),
            "caixa" => funcs.FirstOrDefault(f => f.Cargo == "Caixa"),
            "frentista" => funcs.FirstOrDefault(f => f.Cargo == "Frentista"),
            "administrativo" or "rh" or "auxiliar" => funcs.FirstOrDefault(f => f.Cargo == "RH" || f.Cargo == "Auxiliar Administrativo"),
            _ => funcs.FirstOrDefault()
        };
    }

    public IReadOnlyList<Funcionario> ListarUsuariosDisponiveis() => _repository.ObterFuncionarios();
}
