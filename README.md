# GymTastic
Manual de Instalação – Gymtastic
1. Introdução
Este manual destina-se a administradores técnicos ou programadores responsáveis pela instalação, configuração e manutenção do sistema Gymtastic. Inclui instruções detalhadas para colocar o sistema a funcionar corretamente num ambiente local ou servidor.
2. Pré-requisitos
2.1 Software Necessário
- .NET 6.0 SDK ou superior
- SQL Server (Express ou Standard)
- Visual Studio 2022 ou superior (recomendado)
- Entity Framework Core
- Git (para versionamento, se aplicável)
2.2 Hardware Recomendado
- CPU: Dual-core ou superior
- RAM: 4 GB (mínimo), 8 GB recomendado
- Armazenamento: 1 GB livre
3. Configuração da Base de Dados
1. Abrir o SQL Server Management Studio (SSMS).
2. Criar uma nova base de dados com o nome 'GymTastic'.
3. Executar o script SQL fornecido (`GymTastic.sql`) para criar as tabelas e relações.
4. Confirmar que todas as tabelas foram criadas com sucesso.
4. Instalação da Aplicação
1. Clonar ou extrair o projeto Gymtastic para o seu ambiente local.
2. Abrir a solução `.sln` no Visual Studio.
3. Verificar o ficheiro `appsettings.json` e configurar a `ConnectionString` para apontar à base de dados local.
4. Executar `Update-Database` na Package Manager Console para aplicar as migrations (se necessário).
5. Executar o projeto a partir do Visual Studio (`F5` ou `Ctrl+F5`).
5. Instalação em Ambiente de Produção
1. Publicar o projeto via Visual Studio (Publish > Folder/Profile).
2. Colocar os ficheiros num servidor com IIS configurado.
3. Criar um site no IIS apontando para a pasta publicada.
4. Verificar permissões e configurar a base de dados da mesma forma que em ambiente local.
