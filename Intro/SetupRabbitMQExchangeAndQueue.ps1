param([String]$RabbitDllPath = "not specified")
Set-ExecutionPolicy Unrestricted

Write-Host "Rabbit DLL Path: ”

Write-Host $RabbitDllPath -foregroundcolor green

$absoluteRabbitDllPath = Resolve-Path $RabbitDllPath


Write-Host "Absolute Rabbit DLL Path: "

Write-Host $absoluteRabbitDllPath —foregroundcolor green

[Reflection.Assembly]::LoadFile($absoluteRabbitDllPath)

Write-Host "Setting up RabbitMQ Connection Factory"

$factory = new-object RabbitMQ.Client.ConnectionFactory

$hostNameProp = [RabbitMQ.Client.ConnectionFactory].GetField(“HostName”)

$hostNameProp.SetValue($factory, ”localhost")

$userNameProp = [RabbitMQ.Client.ConnectionFactory].GetField(“UserName”)

$userNameProp.SetValue($factory, ”guest")

$passwordProp = [RabbitMQ.Client.ConnectionFactory].GetField(“Password”)

$passwordProp.SetValue($factory, ”guest")

$createConnectionMethod = [RabbitMq.Client.ConnectionFactory].GetMethod("CreateConnection",[Type]::EmptyTypes)
$connection = $createConnectionMethod.Invoke($factory,"instance,public",$null,$null,$null)

Write-Host "Setting up Model"
$model = $connection.CreateModel()

Write-Host "Creating Exchange"
$exchangeType = [RabbitMq.Client.ExchangeType]::Fanout
$model.ExchangeDeclare("PublishSubscribeExchange",$exchangeType,$true)


Write-Host "Creating Queue-1"
$model.QueueDeclare("Q1",$true,$false,$false,$null)
$model.QueueBind("Q1","PublishSubscribeExchange","")


Write-Host "Creating Queue-2"
$model.QueueDeclare("Q2",$true,$false,$false,$null)
$model.QueueBind("Q2","PublishSubscribeExchange","")

Write-Host "Setup Complete"

