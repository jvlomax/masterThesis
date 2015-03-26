using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Parse;
using System.Threading.Tasks;
public class Beacons : MonoBehaviour {
    public MapNav map;
    public GameObject BluePointerPrefab;
    public GameObject RedPointerPrefab;
    public static bool DisableMultiThread = true;
    public static bool LogErrors = true;

    private List<ParseGeoPoint> geoPoints;
	// Use this for initialization
	void Start () {
	  //  GameObject point = Instantiate(BluePointerPrefab);
	   
        geoPoints = new List<ParseGeoPoint>();
	    //SetGeolocation location = point.GetComponent<SetGeolocation>();
	    //location.lat = 69.69871f;
	    //location.lon = 19.00821f;
        //point.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
        //point.transform.position = new Vector3(point.transform.position.x, 2, point.transform.position.z);
	    // GameObject pointer = Instantiate(BluePointerPrefab);
	    //SetGeolocation set = pointer.GetComponent<SetGeolocation>();
	    //set.lat = 69.0f;
	    //set.lon = 31f;
	    //Instantiate(BluePointer);

	    
        //updateBeacons();
	    StartCoroutine(updateBeacons());
        
	}


    private IEnumerator updateBeacons() {
        Debug.Log("start");
        ParseQuery<ParseObject> query = ParseObject.GetQuery("Beacon");
        var queryTask = query.FindAsync();
        while (!queryTask.IsCompleted) yield return null;
        Debug.Log("done");
        if(queryTask.IsFaulted)
        {
            Debug.Log("queryTask faulted");
            yield break;
        }
        String team = "";
        GameObject point = null;
        ParseGeoPoint geoPoint;
        IEnumerable<ParseObject> results = queryTask.Result;
        foreach (var obj in results) {
            geoPoint = obj.Get<ParseGeoPoint>("coordinates");

            ParseObject guardian = obj.Get<ParseObject>("guardian");
            var guardianTask = guardian.FetchIfNeededAsync();
            while (!guardianTask.IsCompleted) yield return null;

            team = guardian.Get<String>("team");
            if (team.ToLower() == "red") {
                point = Instantiate(RedPointerPrefab);
            }else if (team.ToLower() == "blue") {
                point = Instantiate(BluePointerPrefab);
            }
            else {
                point = Instantiate(BluePointerPrefab); //TODO: change to black pointer
            }
            
            SetGeolocation pointSetGeolocation = point.GetComponent<SetGeolocation>();
            pointSetGeolocation.lat = (float) geoPoint.Latitude;
            pointSetGeolocation.lon = (float)geoPoint.Longitude;
            pointSetGeolocation.orientation = 180f;
            pointSetGeolocation.height = 500;
        }
        /*
        
        ParseQuery<ParseObject> query = ParseObject.GetQuery("Beacon");
        
        Task myTask = query.FindAsync().ContinueWith(t => {
            if (t.IsFaulted)
            {
                Debug.Log("isFaulted");
                return t;
            }
            
            String team = null;
            GameObject point = null;
            SetGeolocation pointLocation = null;
            ParseGeoPoint coord = new ParseGeoPoint(0,0);
            IEnumerable<ParseObject> results = t.Result;
            Debug.Log("here");
            foreach (var obj in results)
            {
                Debug.Log(obj.Get<String>("location"));
                // point = Instantiate(BluePointerPrefab);
                //pointLocation = point.GetComponent<SetGeolocation>();
                
                
                coord = obj.Get<ParseGeoPoint>("coordinates");
                
                
                Debug.Log("here too");
                Debug.Log("coordinate: " + coord.Latitude);
                
                geoPoints.Add(coord);
                Debug.Log(geoPoints.Count);
                //pointLocation.lat = (float)coord.Latitude;
                //pointLocation.lon = (float)coord.Longitude;
               
            }
            Debug.Log(t.Result);
            return t;
        }).Unwrap().ContinueWith(t => {
            Debug.Log("continue");
            Debug.Log(geoPoints.Count);
            foreach (var obj in geoPoints)
            {
                Instantiate(BluePointerPrefab);
                Debug.Log(obj.Latitude);
            }
        });

        while (!myTask.IsCompleted) yield return null;
        Debug.Log("done");
         * */
    }

	// Update is called once per frame
	void Update () {
	
	}
}
