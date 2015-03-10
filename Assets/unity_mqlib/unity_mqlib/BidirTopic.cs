// This source code is licensed under the Apache License, version 2.0.
//
// The APL v2.0:
//
//---------------------------------------------------------------------------
//   Copyright (C) 2007-2013 GoPivotal, Inc.
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
//---------------------------------------------------------------------------
//

using UnityEngine;

using System.Collections;
using System.Threading;
using System.Text;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using RabbitMQ.Util;


public class BidirTopic : MonoBehaviour {
	public string serverip;
	public string ToClientEx;
	public string FromClientEx;

	
	ConnectionFactory cf;
	IConnection conn;
	IModel ch = null;
	QueueingBasicConsumer consumer;
	
	System.Collections.Queue queue = null;
	string lastMessage;
	public GameObject bombType;
	public AudioClip audioClip;

	IModel chFC = null;
	bool b_waitrelease;

	// Use this for initialization
	void Start()
	{
		cf = new ConnectionFactory();
		cf.HostName = serverip;
		conn = cf.CreateConnection();
		
		conn.ConnectionShutdown += new ConnectionShutdownEventHandler(LogConnClose);
		
		ch = conn.CreateModel();
		ch.ExchangeDeclare(ToClientEx, "topic",false,true,null);
		string queueName = ch.QueueDeclare();
		
		ch.QueueBind(queueName, ToClientEx, "#");
		consumer = new QueueingBasicConsumer(ch);
		ch.BasicConsume(queueName, true, consumer);
		
		queue = new System.Collections.Queue();
		queue.Clear();

		// setup sender
		chFC = conn.CreateModel ();
		ch.ExchangeDeclare (FromClientEx, "topic", false, true, null);
		
		b_waitrelease = false;
	}
	
	// Update is called once per frame
	void Update()
	{
		if (ch == null || consumer == null) return;
		BasicDeliverEventArgs ea;

		// Input.GetMouseButtonDown(0)でマウスクリック取得
		if (b_waitrelease == false && Input.GetMouseButtonDown(0)){
			// request to send message
			var body = Encoding.UTF8.GetBytes("move {1,1}");
			//ch.BasicPublish(chFC, severity, null, body);
			chFC.BasicPublish(FromClientEx, "move.map.9999", null, body);
			//Console.WriteLine(" [x] Sent '{0}':'{1}'", "null", message);
			b_waitrelease = true;
		}
		if (Input.GetMouseButtonUp(0)){
			b_waitrelease = false;
		}

		while ((ea = (BasicDeliverEventArgs)consumer.Queue.DequeueNoWait(null)) != null)
		{
			var body = ea.Body;
			var routingKey = ea.RoutingKey;
			var message = Encoding.UTF8.GetString(body);
			queue.Enqueue(message + " from " + routingKey);
			Debug.Log(message + " from " + routingKey);
			//if (routingKey.StartsWith("move")) {
				this.BlastOnce();
			//}
		}
	}
	
	void OnGUI()
	{
		if (queue != null && queue.Count > 0)
		{
			lastMessage = queue.Dequeue().ToString();
		}
		GUILayout.Label((string)lastMessage);
	}
	
	public static void LogConnClose(IConnection conn, ShutdownEventArgs reason)
	{
		Debug.Log("Closing connection normally. " + conn + " with reason " + reason);
	}

	void BlastOnce () {
		Instantiate(bombType, this.transform.position, this.transform.rotation);
		AudioSource.PlayClipAtPoint(audioClip, new Vector3());
	}
}
