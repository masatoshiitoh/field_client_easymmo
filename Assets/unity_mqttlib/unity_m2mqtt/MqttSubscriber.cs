using UnityEngine;

using System.Collections;
using System.Threading;

using uPLibrary.Networking.M2Mqtt;
using System;
using System.Runtime.InteropServices;
using System.Text;
using uPLibrary.Networking.M2Mqtt.Messages;

public class MqttSubscriber : MonoBehaviour {
	private MqttClient client = null;
	
	public string brokerHostname = null;
	public int brokerPort = 1883;
	public string userName = null;
	public string password = null;

	public string topic = "unity";

	System.Collections.Queue queue = null;
	string lastMessage;
	
	void Start () {
		if (brokerHostname != null && userName != null && password != null) {
			Connect (brokerHostname, brokerPort, userName, password);
			Subscribe(topic);
		}
		queue = new System.Collections.Queue();
		queue.Clear();
	}
	
	void Update () {
		if (client == null) return;
	}

	public void Connect(string bhost, int bport, string uname, string pw)
	{
		client = new MqttClient(bhost, bport, false, null);
		string clientId = Guid.NewGuid().ToString();
		client.Connect(clientId, uname, pw);
	}

	private void onReceive(object sender,
	                       uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
	{
		Console.WriteLine(e.Topic);
		string msg = Encoding.UTF8.GetString(e.Message);
		queue.Enqueue(msg);
	}
	
	public void Subscribe(string topicName){
		client.MqttMsgPublishReceived += this.onReceive;
		client.Subscribe(new string[] { topicName }, new byte[] {
			MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
	}
	
	public void Publish(string topicName, string msg)
	{
		client.Publish(
			topicName, Encoding.UTF8.GetBytes(msg),
			MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, false);
	}

}
