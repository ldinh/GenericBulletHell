//var amplitude = 1.0;
var LT : Light;
var LightOn = true;
var GlowOn = true;
var DustOn = false;
var MouseOn = false;
var Dust : GameObject;

var LightTXT = "Light Off";
var GlowTXT = "Glow On";
var DustTXT = "Dust and Fog On";



function OnGUI() {
	if (GUI.Button(Rect(Screen.width/2+100,10,100,50),LightTXT)){
			if(LightOn){
			LightOn = false;
			LightTXT = "Light On";
			LT.GetComponent.<Light>().enabled = false;			
			}
			else{
			LightOn = true;
			LightTXT = "Light Off";
			LT.GetComponent.<Light>().enabled = true;
			
			}
	}
	
	if (GUI.Button(Rect(Screen.width/2-100,10,100,50),GlowTXT)){
			if(GetComponent(GlowEffect).enabled){
			GlowOn = false;
			GlowTXT = "Glow On";
			GetComponent(GlowEffect).enabled = false;
					
			}
			else{
			GlowOn = true;
			GlowTXT = "Glow Off";
			GetComponent(GlowEffect).enabled = true;			
			}
	}
	
	//Dust and fog
	if (GUI.Button(Rect(Screen.width/2-300,10,120,50),DustTXT)){
			if(RenderSettings.fog){
			DustOn = false;
			RenderSettings.fog = false;
			DustTXT = "Dust and Fog On";
			Dust.active = false;								
			}
			else{
			DustOn = true;
			RenderSettings.fog = true;
			DustTXT = "Dust and Fog Off";
			Dust.active = true;
			}
	}

	
}//end gui

