# COMANDOS PARA INICIAR PROJETO .NET COM IDENTITY NO VS Code

Comando para criar solution file no VSCode:
    dotnet new sln -n -NomeDaSolucao- -o -NomePasta-

Criar Projeto do Tipo MVC com Identity (Entrada Individual):
    dotnet new mvc -n -NomeDoProjeto- -au Individual

Adicionar projeto à solução:
    dotnet sln add .\CaminhoDoProjeto\

Restauração de Pacotes NuGet:
    dotnet restore

Migração para banco de dados: 
    dotnet ef database update

Criar arquivo .gitignore:
    dotnet new gitignore

# Projetos com Identity criados no VSCODE pelo terminal e pelo Visual Studio.

# Comandos para instalação de pacotes pelo Package Manager Console:
- install-package Microsoft.AspnetCore.Identity
- Entity framework para Identity: install-package Microsoft.AspnetCore.Identity.EntityFrameworkCore





