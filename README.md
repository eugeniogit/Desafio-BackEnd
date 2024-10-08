# Desafio backend Mottu.
Esta é o desafio feito por Eugênio.

## Instruções para rodar o projeto
- Abra o terminal no diretório da solução.
- Rodar o comando "docker-compose up" para instalação dos serviços.
- Após a instalação, certificar que os serviços (containers) estajam instalados e rodando, pois é fundamento para o teste end-to-end do aplicativo.
  
## Executando o aplicativo
- Abra o terminal na pasta "MTT.WebApi"
- Rodar o comando "dotnet run" para subir o aplicativo
- Chamar o endereço "http://localhost:5201/swagger/index.html" no navegador

OBS 1.: O banco de dados será criado durante o start do serviço via Migration.

## Integrações
- Foi criado um aplicativo Worker para processamento dos eventos de integrações
- Para acompanhar a integração, abra o terminal na pasta "MTT.MotoWorker"
- Rodar o comando "dotnet run" para subir o worker

## Observações do desenvolvimento

- Não foi possível concluir a integração com o S3 bucket por conta de alguns desafios na comunicação do localstack, mas o código de implementação foi feito e está comentado na classe S3StorageService

- Por conta do prazo, não foi possível concluir 100% dos testes de integrações e logs.

- Algumas dúvidas surgiram nos requisitos das datas das reservas, mas deu pra pegar a visão.

- Por fim, ficou algumas oportunidades de refatoração, como por exemplo, atribuir os DTOs como parametros dos metódos das classes de serviços, e outras.
