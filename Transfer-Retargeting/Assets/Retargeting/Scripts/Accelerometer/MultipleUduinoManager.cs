using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Uduino;

class BoardValues {
	public int hits;
	public float accMag;
}

public class MultipleUduinoManager : MonoBehaviour
{
    public bool calibrate = false;
	Dictionary<UduinoDevice, BoardValues> boards;

    // Start is called before the first frame update
    void Start() {
    	boards = new Dictionary<UduinoDevice, BoardValues>();

        UduinoManager.Instance.OnBoardConnected += BoardConnected;
        UduinoManager.Instance.OnBoardDisconnected += BoardDisonnected;
        UduinoManager.Instance.OnValueReceived += ValueReceived;
    }

    // Update is called once per frame
    void Update() {
    	/*foreach (KeyValuePair<UduinoDevice, BoardValues> board in boards) {
    		print(board.Value.accMag);
    	}*/

        if (calibrate==true) {
            calibrate = false;
            BroadcastCommand("Calibrate");
        }
    }

    public void BroadcastCommand(string commandName) {
    	foreach (KeyValuePair<UduinoDevice, BoardValues> board in boards) {
    		UduinoManager.Instance.sendCommand(board.Key, commandName);
    	}
    }

    public void BroadcastCommand(string commandName, int argument) {
    	foreach (KeyValuePair<UduinoDevice, BoardValues> board in boards) {
    		UduinoManager.Instance.sendCommand(board.Key, commandName, argument);
    	}
    }

    public List<int> GetHitCount() {
        List<int> tmp = new List<int>();
        foreach (KeyValuePair<UduinoDevice, BoardValues> board in boards) {
            tmp.Add(board.Value.hits);
        }
        return tmp;
    }

    public List<float> GetAcceleration() {
        List<float> tmp = new List<float>();
        foreach (KeyValuePair<UduinoDevice, BoardValues> board in boards) {
            tmp.Add(board.Value.accMag);
        }
        return tmp;
    }

    void BoardConnected(UduinoDevice device) {
    	boards.Add(device, new BoardValues());
    	print("New Board Connected : " + boards.Count);
    }

    void BoardDisonnected(UduinoDevice device) {
    	boards.Remove(device);
    	print("Board Disconnected : " + boards.Count);
    }

    void ValueReceived(string value, UduinoDevice board) {
    	string[] values = value.Split(' ');
		float.TryParse(values[0], NumberStyles.Any, CultureInfo.InvariantCulture , out boards[board].accMag);
		if (values.Length==2)
    		int.TryParse(values[1], out boards[board].hits);
    }
}
