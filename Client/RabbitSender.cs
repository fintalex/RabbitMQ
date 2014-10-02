using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
	class RabbitSender
	{
		private const string HostName = "localhost";
		private const string UserName = "guest";
		private const string Password = "guest";
		private const string QueueName = "OneWayQueue";
		private const string ExchangeName = "";
		private const bool IsDurable = true;
		// the two below settings are just to illustrate how they can be used
		//this sample as we will use the defaults
		private const string VirtualHost = "";
		private int Port = 0;

		private ConnectionFactory _connectionFactory;
		private IConnection _connection;
		private IModel _model;

		public RabbitSender()
		{
			DisplaySettings();
			SetupSettings();
		}

		private void DisplaySettings()
		{
			Console.WriteLine("Host: {0}", HostName);
			Console.WriteLine("UserName: {0}", UserName);
			Console.WriteLine("Password: {0}", Password);
			Console.WriteLine("QueueName: {0}", QueueName);
			Console.WriteLine("ExchangeName: {0}", ExchangeName);
			Console.WriteLine("IsDurable: {0}", IsDurable);
			Console.WriteLine("VirtualHost: {0}", VirtualHost);
			Console.WriteLine("Port: {0}", Port);
		}

		private void SetupSettings()
		{
			_connectionFactory = new ConnectionFactory
			{
				HostName = HostName,
				UserName = UserName,
				Password = Password
			};

			if (string.IsNullOrEmpty(VirtualHost) == false)
			{
				_connectionFactory.VirtualHost = VirtualHost;
			}
			if (Port > 0)
			{
				_connectionFactory.Port = Port;
			}

			_connection = _connectionFactory.CreateConnection();
			_model = _connection.CreateModel();
		}

		public void Send(string message)
		{
			// setup properties
			var properties = _model.CreateBasicProperties();
			properties.SetPersistent(true);

			//serialize
			byte[] messageBuffer = Encoding.Default.GetBytes(message);

			//send message
			_model.BasicPublish(ExchangeName, QueueName, properties, messageBuffer);
		}
	}
}
