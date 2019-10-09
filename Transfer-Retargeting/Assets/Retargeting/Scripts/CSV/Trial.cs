using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Trial  {
	//int participant;
	public string[] parameters;
	public int id;

	public Trial(string[] param) {
		this.parameters = param;
		this.id = GetId();
	}

	public bool Equals(Trial other) {
        if (other == null) return false;
		if (this.id.Equals(other.id)) {
			return true;
		}
        return false;
    }

    public void print() {
    	string str = "";
    	foreach(string s in parameters)
    		str += ", " + s;
    	Debug.Log("ID : " + id + ", " + str);
    }

    int GetId() {
		if (this.parameters[0]=="Participant" || this.parameters[0]=="") {
			return -1;
		}
        return  Int32.Parse(this.parameters[0])
			  + Int32.Parse(this.parameters[2])*Parameters.blockFactor
  			  + Int32.Parse(this.parameters[3])*Parameters.trialFactor;
    }
}
