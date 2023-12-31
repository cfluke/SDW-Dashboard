using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using Logger = Logs.Logger;

public sealed class TCPHandler {
	private static TCPHandler instance;
	private static object instanceLock = new object();

	private readonly TcpListener listener;
	private readonly Thread listenerThread;

	private readonly Dictionary<string, TcpClient> clients = new Dictionary<string, TcpClient>();
	private readonly object clientDictionaryLock = new object();

	private const int port = 8000;

	private TCPHandler() {
		listener = new TcpListener(System.Net.IPAddress.Any, port);

		// TODO; figure out if this should be after starting the listen thread starts; don't think it will matter but better to check
		listener.Start();

		ThreadStart threadRef = new ThreadStart(Listen);

		listenerThread = new Thread(threadRef);

		listenerThread.Start();
	}

	// Return determines if the instance was started or already existed
	public static bool Instantiate() {
		if (instance != null) {
			return false;
		}

		lock (instanceLock) {
			if (instance != null) {
				return false;
			}

			instance = new TCPHandler();
		}

		return true;
	}

	// Bool determines is message was sent; does not know if it was received
	public static bool SendMessage(string id, ServerToClientMessage message) {
		if (!instance.clients.ContainsKey(id)) {
			return false;
		}

		lock (instance.clientDictionaryLock) {
			// Thread safety pattern; ensures key is still there
			if (!instance.clients.ContainsKey(id)) {
				return false;
			}

			NetworkStream ns = instance.clients[id].GetStream();

			SendMessageUnsafe(ns, message);
		}

		return true;
	}

	// Unsafe as this does not require the client to be identified; internal use only
	private static void SendMessageUnsafe(NetworkStream ns, ServerToClientMessage message) {
		byte[] messageBytes = Encoding.ASCII.GetBytes("    " + JsonUtility.ToJson(message));

		int length = messageBytes.Length - 4;

		Array.Copy(BitConverter.GetBytes(length), messageBytes, 4);

		ns.Write(messageBytes, 0, messageBytes.Length);
	}

	// Handles a new client joining
	#nullable enable
	private void HandleClient(object clientObject) {
		TcpClient client = (TcpClient)clientObject;
		string? clientID = null;
		NetworkStream ns = client.GetStream();

		Logger.Instance.Log("New client");

		Debug.Log("test1");
		// Wait for the client to be fully connected
		while (!client.Connected) { }
		Debug.Log("test2");

		// SendMessage is not used here, as it requires the client to have already been identified
		// Request the client identifies itself
		SendMessageUnsafe(ns, new ServerToClientMessage {
			MessageType = MessageTypes.RequestIdentify,
			payload = "",
		});

		// While the client is connected; read its messages
		while (client.Connected) {
			// Bytes relating to incoming message size
			byte[] message = new byte[4];

			ns.Read(message, 0, 4);

			// Convert bytes into int containing incoming message size
			int messageLength = BitConverter.ToInt32(message, 0);

			// Buffer for incoming message
			message = new byte[messageLength];

			ns.Read(message, 0, messageLength);

			// Fire message received event
			OnMessageReceived(clientID, JsonUtility.FromJson<ClientToServerMessage>(Encoding.Default.GetString(message)), client);
		}

		// This is reached once the client disconnects
		Logger.Instance.Log("Client disconnected");

		// Artificially creates the disconnect message
		string disconnectMessage = JsonUtility.ToJson(new ClientToServerMessage {
			MessageType = MessageTypes.Disconnect,
			payload = "",
		});

		byte[] disconnectBytes = Encoding.ASCII.GetBytes(disconnectMessage);

		OnMessageReceived(clientID, JsonUtility.FromJson<ClientToServerMessage>(Encoding.Default.GetString(disconnectBytes)), client);

		// Remove client from dictionary
		lock (clientDictionaryLock) {
			clients.Remove(clientID);
		}
	}
	#nullable disable

	// When a new client joins, make a new thread to handle its messages
	private void Listen() {
		while (true) {
			TcpClient client = listener.AcceptTcpClient();

			ThreadPool.QueueUserWorkItem(HandleClient, client);
		}
	}

	public static event EventHandler<TCPMessageReceivedEventArgs> MessageReceived;

	#nullable enable
	private void OnMessageReceived(string? clientID, ClientToServerMessage message, TcpClient client) {
		// Artificially handle indentification messages here before anywhere else
		if (message.MessageType == MessageTypes.Identify) {
			IdentifyMessage identifyMessage = JsonUtility.FromJson<IdentifyMessage>(message.payload);
			
			lock (clientDictionaryLock) {
				// TODO: prevent attempting to add ID that is already taken
				clients.Add(identifyMessage.id, client);
			}

			clientID = identifyMessage.id;
		}

		// If the client is not yet identified; prevent the message from firing
		if (clientID == null) {
			Logger.Instance.LogError("Recieved message from client that has not yet identified itself, ignoring message");
			// TODO: Send message to client telling it the request failed due to lack of ID
			return;
		}

		// Create the message event, then fire the event
		TCPMessageReceivedEventArgs e = new TCPMessageReceivedEventArgs(clientID, message);

		Logger.Instance.Log("Message received\n\tType; " + e.message.MessageType + "\n\tContents; " + e.message.payload + "\n");
		MessageReceived?.Invoke(this, e);
	}
	#nullable disable

}

public enum MessageTypes {
	Identify,
	Disconnect,
	RequestIdentify,
	RequestEcho,
	Echo,
	StartApp,
	StopApps
}

// Shouldn't actually need two different message types; but it's here just in case
[Serializable]
public struct ServerToClientMessage {
	public MessageTypes MessageType {
		get {
			MessageTypes ret;
			if (Enum.TryParse<MessageTypes>(messageType, out ret)) {
				return ret;
			}
			throw new Exception("Attempted to parse a Message Type that does not exist");
		}
		set {
			messageType = value.ToString();
		}
	}
	[SerializeField]
	private string messageType;
	[SerializeField]
	public string payload;
}

[Serializable]
public struct ClientToServerMessage {
	public MessageTypes MessageType {
		get {
			MessageTypes ret;
			if (Enum.TryParse<MessageTypes>(messageType, out ret)) {
				return ret;
			}
			throw new Exception("Attempted to parse a Message Type that does not exist");
		}
		set {
			messageType = value.ToString();
		}
	}
	[SerializeField]
	private string messageType;
	[SerializeField]
	public string payload;
}

public class TCPMessageReceivedEventArgs : EventArgs {
	public readonly ClientToServerMessage message;
	public readonly string clientID;

	public TCPMessageReceivedEventArgs(string client, ClientToServerMessage message) {
		this.message = message;
		this.clientID = client;
	}
}

[Serializable]
public class IdentifyMessage
{
	public string id;
	public string ip;
	public string path;
	public DisplayDetails[] displayDetails;
}

[Serializable]
public class DisplayDetails
{
	public int x, y;
	public int w, h;
}