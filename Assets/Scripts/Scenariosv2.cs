using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
// using UnityEngine.Debug;
using PathCreation;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
// using Rotator;
public class Scenariosv2 : MonoBehaviour
{
     private static string kTAG2 = "Vronsky"; 

     /////////////////////////////////////CONTROLLERS, CAMERA RIGS, DISPLAY//////////////////////////////
    [SerializeField] GameObject leftController, rightController, leftHand, rightHand;
    [SerializeField] GameObject cameraCafe, displayCafe, standingPerson;
    string user_perspective;
    int scenario_previous, scenario_current, scenario_current_max;
    OVRCameraRig cam;
    float TimeAmount, currentTime;
    int last_scenario_Cafe;
    bool activeScreen = false;
    Text txt = null;
    bool justChangedScenario= false;
    //////////////////////////////////////PROJECTION//////////////////////////////////////
    [SerializeField]  GameObject projection_stem, projection_arrow;
    float spacing = 0.035f;
    const float minSpacing = 0.01f;

    ///////////////////////////////////////PATHS, SPEED//////////////////////////////////////////
    private bool runCar; 
    [SerializeField] PathCreator pathCreator0, pathCreator1, pathCreator2, pathCreator3, pathCreator4, pathCreator5, pathCreator6, pathPlatoon_, 
    pathCreator8, pathCreator9, pathCreatorPlatoonStanding10, pathCreator11, pathCreator12, pathCreator13, pathCreator14,pathCreator15,pathCreator16,pathCreator17, pathCreator18, PathCreator19;
    VertexPath path0, path1, path2, path3, path4, path5, path6, pathPlatoon, path9, path8, pathPlatoonStanding10, path11, path12, path13, path14, path15, path16, path17, path18, path19;
    EndOfPathInstruction end = EndOfPathInstruction.Stop;
    // float speedAvg = 2.4f;// 1.2f; //original was 1
    // float speedSlow = 0.9f;
    // float speedFast = 1.5f;

    private const float speedAvg = 1.4f;//1.8f;
    float speedSlow = 1.2f;
    float speedFast = 2.4f;

    ///////////////////////////////////////MOVING-ENTITIES//////////////////////////////////////////
    [SerializeField] GameObject carEmpty;
    [SerializeField] GameObject carBryce, carElizabeth, carBrian, carJody, carDavid, carSuzie, carRoth, carLouise;
    [SerializeField] GameObject Lewis, Megan, Josh, Sophie, Martha, Shannon, Adam, Kate, Joe, headlessCafeGuy;
    int currPlaying;
    public struct entityStruct{ public GameObject carLocal; public VertexPath pathLocal; public float speedLocal; public bool projecting; public GameObject holderLocal; public bool numRepeatMovement;}
    entityStruct thisPerson0, thisPerson1, thisPerson2, thisPerson3, thisPerson4, thisPerson5, thisPerson6, thisPerson7, thisPerson8, thisPerson9;
    float dstTravelledPerson0, dstTravelledPerson1, dstTravelledPerson2, dstTravelledPerson3, dstTravelledPerson4, dstTravelledPerson5, dstTravelledPerson6, dstTravelledPerson7, dstTravelledPerson8, dstTravelledPerson9;
    entityStruct thisCar0, thisCar1, thisCar2, thisCar3, thisCar4, thisCar5, thisCar6, thisCar7;
    float dstTravelled0, dstTravelled1, dstTravelled2, dstTravelled3, dstTravelled4, dstTravelled5, dstTravelled6, dstTravelled7;
    entityStruct[] entities;
    Stopwatch stopWatch;

    ////////////////////////////////COLOR-SHIRTS///////////////////////////////////////////
    [SerializeField] Material materialBryce, materialMegan, materialElizabeth, materialJosh, material5,material6,material7, material8,material9, material10,material11,material12, material13, material14, material15, material16;

    //5-10 PEDESTRIANS
    //11-16 PASSENGERS
    Material materialCurrent;
    int colorCount, colorCountMax;
    Color[] colorsShirts;
    IDictionary<GameObject, Material> materialMapping;
    List<string> currentEntitiesList = new List<string>();
    [SerializeField] Timer timer;
    // Start is called before the first frame update

/*0*/    object [,] scenarios = { {1, "none   ",  true},   //1: ONE pod with a passenger 
/*1*/                             {1, "none   ",  false},  //2: ONE pod no passenger
 /*2*/                            {3, "scatter", true},    //3: THREE scattered pods WITH passengers
 /*3*/                            {3, "platoon", true},    //4: THREE platoon WITH Passengers
 /*4*/                            {3, "scatter",  false},  //5: THREE scattered NO passenger
 /*5*/                            {3, "platoon",  false},  //6: THREE platoon NO passenger
 /*6*/                            {6, "scatter",  true},   //7: SIX scattered pods WITH passengers
 /*7*/                            {6, "platoon",  true},   //8: SIX platoon WITH Passengers
 /*8*/                            {6, "scatter",  false},  //9: SIX scattered NO passenger
 /*9*/                            {6, "platoon",  false}};//10:SIX platoon NO passenger
                    
object [] currScenario;
int numPods, numPedestrians;
string formation;
bool isPassenger; //isProjection, 
int []randScenarioOrder;
int setorder;
VertexPath [,] setorderArr;

[SerializeField] GameObject Bryce, Passenger;
GameObject [] pedestrianArr, passengerArr;

bool breakON;

string firstSet, secondSet;

    void Start()
    {
        pedestrianArr = new GameObject [] { Megan, Josh, Sophie, Martha, Shannon, Adam, Kate, Joe};
        passengerArr = new GameObject [] {carBryce, carElizabeth, carBrian, carJody, carDavid, carSuzie, carRoth, carLouise};

        // randScenarioOrder = new int []{1,2,3,4,5,6,7,8,9,10, 1,2,3,4,5,6,7,8,9,10};
    //    randScenarioOrder = new int []{7,4};
         //   randScenarioOrder = new int []{8};

        ////////////////////////////////////////////


        // //P10 sit, then stand
        // randScenarioOrder = new int []{4, 5, 3, 6, 2, 7, 1, 8, 10, 9, 8, 9, 7, 10, 6, 1, 5, 2, 4, 3};

        randScenarioOrder = new int []{10,9,8,7,6};
        setorder = 0;

       //presets for scenarios
        scenario_current = -1; //nothing happens here, to start scenario 1, press the A or X button.
        scenario_current_max = 19; //how many scenarios I am presenting
        scenario_previous = -2;
        last_scenario_Cafe = 1;//10;
        //screen presets how long screen is displayed
        TimeAmount = 2f; 
        currentTime = TimeAmount;
        getDisplay();

        //camera perspective
 // "fountain_standing" 
        cam = GameObject.Find("OVRCameraRig").GetComponent<OVRCameraRig>();

        leftController.SetActive(false);
        rightController.SetActive(false);
        //set camera
        cameraCafe.SetActive(true);
        displayCafe.SetActive(false);
        standingPerson.SetActive(false);

        //path stuff
        end = EndOfPathInstruction.Stop;
        path0 = pathCreator0.path;
        pathPlatoon = pathPlatoon_.path;
        path1 = pathCreator1.path;
        path2 = pathCreator2.path;
        path3 = pathCreator3.path;
        path4 = pathCreator4.path;
        path5 = pathCreator5.path;
        path6 = pathCreator6.path;
        path8 = pathCreator8.path;
        path9 = pathCreator9.path;
        pathPlatoonStanding10 = pathCreatorPlatoonStanding10.path;
        path11 = pathCreator11.path;
        path12 = pathCreator12.path;
        path13 = pathCreator13.path;
        path14 = pathCreator14.path;
        path15 = pathCreator15.path;
        path16 = pathCreator16.path;
        path17 = pathCreator17.path;
        path18 = pathCreator18.path;
        path19 = PathCreator19.path;


        setorderArr = new VertexPath [,] {{path0, path1, path2, path3, path4, path5, path6, pathPlatoon, path8, path9, pathPlatoonStanding10, path11, path12, path13, path14, path15,  path16,  path17, path18, path19}};

        runCar = false; //car not moving to start with

        colorCount = 0;
        materialMapping = new Dictionary<GameObject, Material>(){{carBryce, materialBryce}, {carElizabeth, materialElizabeth}, {Megan, materialMegan}, {Josh, materialJosh},
        {Adam, material5}, {Kate, material6}, {Martha, material7}, {Shannon, material8}, {Joe, material9}, {Sophie, material10},
        {carBrian, material11}, {carJody, material12}, {carLouise, material13}, {carDavid, material14}, {carSuzie, material15}, {carRoth, material16}};
        Color navy =  new Color(0.3f, 0.4f, 0.6f, 0.3f);
        Color orange = new Color(0.9f, 0.3f, 0.1f);
        Color green = new Color(0.4f, 0.75f, 0.33f);
        colorsShirts = new Color[]{navy, new Color32(170, 100, 70,1), new Color32(70, 70, 128, 1), Color.grey,  new Color32(106, 101, 36,1), Color.black, new Color32(60, 125, 190,1), new Color32(179, 120, 200, 1), new Color32(250, 107, 67, 1), 
        new Color32(203, 139, 204, 1), new Color32(111, 85, 85, 1), new Color32(77,100, 179,1), new Color32(19, 46, 27,1), new Color32(24, 150, 142,1), new Color32(156, 140, 93,1),
        new Color32(99,50,3,1), new Color32(130, 38, 5,1)};
        colorCountMax = colorsShirts.Length;   

        //start SITTING
            // if(scenario_current >=0 && scenario_current < 10){
            //     user_perspective = "cafe_sitting";
            // }else{
            //     user_perspective = "fountain_standing";
            // }

        firstSet = "cafe_sitting";//"fountain_standing";//
        secondSet = "fountain_standing";//
            // start STANDING
            if(scenario_current >=-2 && scenario_current < 10){
                // print(firstSet + scenario_current);
                user_perspective = firstSet; 
            }else{
                // print(secondSet + scenario_current);
                user_perspective = secondSet;
            } 
            breakON = false;
    }

public void FindEveryChild(Transform parent, List<Transform> childs)
    {
        int count = parent.childCount;
        for (int i = 0; i < count; i++)
        {
            childs.Add(parent.GetChild(i));
        }
    }

    void LateUpdate(){
        ifCarExistsRunIt();
    }


    // Update is called once per frame
    void Update()
    {
        scenarioCounterKeyboard();
        if(scenario_previous != scenario_current){
            print("it took this scenario #" + scenario_previous + " to run: " +  timer.disvar.text + " seconds");
            runCar = true;
            destroyAllEntities();
            changePosition();
            scenario_previous = scenario_current;
            timer.resetbutton();
            timer.startbutton();
            BreakDownScenario();
        }
        if(breakON){
            destroyAllEntities();
            changePosition();
        }
        displayText();
        // leftController.GetComponent<MeshRenderer>().enabled = false;
        // rightController.GetComponent<MeshRenderer>().enabled = false;
        leftController.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
        rightController.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
        leftHand.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f); 
        rightHand.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f); 
    }



////////////////////////////////////////////////////////////////////////////HEAD//////////////////////////////////////////////////////////////////////////////////////////
        void BreakDownScenario(){
            currPlaying = randScenarioOrder[scenario_current];
            // currScenario = randScenarioOrder[]; //change this to randmo and Latin square
            // currScenario = new object[] {3, "scatter", true, 5}; //test run one
            print("WHICH NUMBER WE ARE RETRIEVING IN ARRAY: " + currPlaying);
            numPods = (int) scenarios[currPlaying-1, 0];
            formation = (string) scenarios[currPlaying-1, 1];
            isPassenger = (bool) scenarios[currPlaying-1, 2];

            //isProjection = (bool) scenarios[currPlaying-1, 2];
            //numPedestrians = (int) scenarios[currPlaying-1, 3];

            runCar = false;
            if(numPods == 1){
                Scenario1Pod();
            } else if(numPods == 3){
                Scenario3Pods();
            } else if(numPods == 6){
                Scenario6Pods();
            }
    }

void Scenario1Pod(){
    dstTravelled0 = 0;
    System.Random rnd;
    if(isPassenger){
         ////////////////////////////////////////////////////////!!!!!!!!!!!!!!!!!create diff people on the vehicle (maybe at least 6 people)
        rnd = new System.Random(); 
        var randomIndex = rnd.Next(0, passengerArr.Length);
    //    thisCar0 = createEntity(path0, passengerArr[randomIndex]);

    }else{
        thisCar0 = createEntity(path0, carEmpty);
    }

    rnd = new System.Random(); 
    GameObject[] rndPedestrianArr = pedestrianArr.OrderBy(i => rnd.Next()).Take(6).ToArray();
           stopWatch = new Stopwatch();
       stopWatch.Start();

        // thisPerson0 = createEntity(setorderArr[setorder, 2], rndPedestrianArr[0], isPerson:true);






        thisPerson1 = createEntity(setorderArr[setorder, 3], rndPedestrianArr[1], isPerson:true);
        // thisPerson2 = createEntity(setorderArr[setorder, 6], rndPedestrianArr[2], isPerson:true);
        // // thisPerson3 = createEntity(setorderArr[setorder, 4], rndPedestrianArr[3], isPerson:true);
        // // thisPerson4 = createEntity(setorderArr[setorder, 1], rndPedestrianArr[4], isPerson:true);
        // thisPerson5 = createEntity(setorderArr[setorder, 14], rndPedestrianArr[3], isPerson:true);
        // thisPerson6 = createEntity(setorderArr[setorder, 5], rndPedestrianArr[4], isPerson:true);
        // // thisPerson7 = createEntity(setorderArr[setorder, 6], rndPedestrianArr[5], isPerson:true);
        // thisPerson8 = createEntity(setorderArr[setorder, 7], rndPedestrianArr[5], isPerson:true);
        // thisPerson9 = createEntity(setorderArr[setorder, 8], rndPedestrianArr[0], isPerson:true);

        // dstTravelledPerson0 = 4;
        dstTravelledPerson1 = 6;
        // dstTravelledPerson2 = 14;
        // dstTravelledPerson3 = 1;
        // dstTravelledPerson4 = 4;
        // dstTravelledPerson5 = 2;
        // dstTravelledPerson6 = 0;
        // dstTravelledPerson7 = 4;
        // dstTravelledPerson8 = 14;
        // dstTravelledPerson9 = 0;
    
    // }

}
void Scenario3Pods(){

////////////RANDOM ORDER (both for platoon and scattered but first car always has passengers)
    System.Random rnd = new System.Random(); 
    GameObject[] passengersRnd = passengerArr.OrderBy(i => rnd.Next()).Take(2).ToArray();
    GameObject [] originalArray = new GameObject []{carEmpty, passengersRnd[0]};
    
    rnd = new System.Random();
    GameObject[] emptyANDPassengers = originalArray.OrderBy(e => rnd.NextDouble()).ToArray();
    GameObject[] shuffledArray = new GameObject []{passengersRnd[1], emptyANDPassengers[0], emptyANDPassengers[1]};

    /////THREE PODS PLATOON  
    if(formation == "platoon"){
        int index = 7;
        // if (user_perspective == "fountain_standing"){
        //     index = 10;
        // } else{
        //      index = 7;
        // }
        if(isPassenger){
                thisCar0 =  createEntity(setorderArr[setorder, index], shuffledArray[0]);
                thisCar1 =  createEntity(setorderArr[setorder, index], shuffledArray[1]);
                thisCar2 =  createEntity(setorderArr[setorder, index], shuffledArray[2]);
                dstTravelled0 = 15;
                dstTravelled1 = 12;
                dstTravelled2 = 9;
        } else{
            //////////////////////////////////////////////HOW TO DEAL WTIH RANDOM ORDER: https://stackoverflow.com/questions/40209769/random-number-generator-pick-3-in-a-range-returns-group-of-2-in-some-iteration
            ///have condition: first seat no person, second seat person in front: alternate between perspectives or btw people? 
                thisCar0 =  createEntity(setorderArr[setorder, index], carEmpty);
                thisCar1 =  createEntity(setorderArr[setorder, index], carEmpty);
                thisCar2 =  createEntity(setorderArr[setorder, index], carEmpty);
                dstTravelled0 = 15;
                dstTravelled1 = 12;
                dstTravelled2 = 9;
        }
        

                rnd = new System.Random(); 
                GameObject[] rndPedestrianArr = pedestrianArr.OrderBy(i => rnd.Next()).Take(8).ToArray();



                if (user_perspective == "fountain_standing"){
                    thisPerson2 = createEntity(setorderArr[setorder, 9], rndPedestrianArr[6],  isPerson:true);
                    thisPerson3 = createEntity(setorderArr[setorder, 14], rndPedestrianArr[7],  isPerson:true);
                    // thisPerson5 = createEntity(setorderArr[setorder, 14], rndPedestrianArr[6],  isPerson:true);
                    // thisPerson4 = createEntity(setorderArr[setorder, 13], rndPedestrianArr[7],  isPerson:true);
                }else{
                    thisPerson2 = createEntity(setorderArr[setorder, 6], rndPedestrianArr[6],  isPerson:true);
                    thisPerson7 = createEntity(setorderArr[setorder, 6], rndPedestrianArr[7],  isPerson:true);
                }


                // thisPerson9 = createEntity(setorderArr[setorder, 8], rndPedestrianArr[5], isPerson:true);
                    thisPerson8 = createEntity(setorderArr[setorder, 15], rndPedestrianArr[0], isPerson:true);
                    // thisPerson3 = createEntity(setorderArr[setorder, 4], rndPedestrianArr[0],  isPerson:true);
                    thisPerson4 = createEntity(setorderArr[setorder, 2], rndPedestrianArr[1], isPerson:true);
                    thisPerson0 = createEntity(setorderArr[setorder, 12], rndPedestrianArr[2],  isPerson:true);
                    thisPerson1 = createEntity(setorderArr[setorder, 3], rndPedestrianArr[3], isPerson:true);
                    thisPerson6 = createEntity(setorderArr[setorder, 5], rndPedestrianArr[4],  isPerson:true);
                    thisPerson9 = createEntity(setorderArr[setorder, 3], rndPedestrianArr[5],  isPerson:true);
                    // thisPerson3 = createEntity(setorderArr[setorder, 4], rndPedestrianArr[3],  isPerson:true);
                    // thisPerson8 = createEntity(setorderArr[setorder, 11], rndPedestrianArr[4],  isPerson:true);
                    


        
                dstTravelledPerson0 = 2;
                dstTravelledPerson1 = 0;
                dstTravelledPerson2 = 14;
                dstTravelledPerson3 = 5;
                dstTravelledPerson4 = 10;
                dstTravelledPerson5 = 2;
                dstTravelledPerson6 = 4;
                dstTravelledPerson7 = 8;
                dstTravelledPerson8 = 0;
                dstTravelledPerson9 = 4;
            // }
    } else{ //THREE PODS scattered

            if(isPassenger){
                thisCar0 =  createEntity(setorderArr[setorder, 1], shuffledArray[0]); //pathPlatoon not bad
                thisCar1 =  createEntity(setorderArr[setorder, 4], shuffledArray[1]);
                thisCar2 =  createEntity(setorderArr[setorder, 5], shuffledArray[2]);
                dstTravelled0 = 0;
                dstTravelled1 = 8;
                dstTravelled2 = 3;
            } else{
                thisCar0 =  createEntity(setorderArr[setorder, 1], carEmpty); //pathPlatoon not bad
                thisCar1 =  createEntity(setorderArr[setorder, 4], carEmpty);
                thisCar2 =  createEntity(setorderArr[setorder, 5], carEmpty);
                dstTravelled0 = 0;
                dstTravelled1 = 8;
                dstTravelled2 = 3;
            }

                rnd = new System.Random(); 
                GameObject[] rndPedestrianArr = pedestrianArr.OrderBy(i => rnd.Next()).Take(6).ToArray();
            
                // thisPerson0 = createEntity(setorderArr[setorder, 2], rndPedestrianArr[0], isPerson:true);
                thisPerson1 = createEntity(setorderArr[setorder, 3], rndPedestrianArr[1], isPerson:true);
                thisPerson2 = createEntity(setorderArr[setorder, 6], rndPedestrianArr[2], isPerson:true);
                // thisPerson3 = createEntity(setorderArr[setorder, 4], rndPedestrianArr[3], isPerson:true);
                thisPerson4 = createEntity(setorderArr[setorder, 2], rndPedestrianArr[4], isPerson:true);
                thisPerson5 = createEntity(setorderArr[setorder, 9], rndPedestrianArr[0], isPerson:true);
                // thisPerson6 = createEntity(setorderArr[setorder, 5], rndPedestrianArr[3], isPerson:true);
                thisPerson7 = createEntity(setorderArr[setorder, 6], rndPedestrianArr[5], isPerson:true);
                thisPerson8 = createEntity(setorderArr[setorder, 3], rndPedestrianArr[3], isPerson:true);
                // thisPerson9 = createEntity(setorderArr[setorder, 8], rndPedestrianArr[5], isPerson:true);
        
                dstTravelledPerson0 = 4;
                dstTravelledPerson1 = 0;
                dstTravelledPerson2 = 12;
                dstTravelledPerson3 = 0;
                dstTravelledPerson4 = 0;
                dstTravelledPerson5 = 0;
                dstTravelledPerson6 = 6;
                dstTravelledPerson7 = 4;
                dstTravelledPerson8 = 4;
                dstTravelledPerson9 = 5;
            }
}

void Scenario6Pods(){
    System.Random rnd;

    //SIX POD PLATOON
    if(formation == "platoon"){

        rnd = new System.Random(); 
        GameObject[] passengersRnd = passengerArr.OrderBy(i => rnd.Next()).Take(5).ToArray();
        GameObject [] originalArray = new GameObject []{carEmpty, passengersRnd[0], passengersRnd[1], passengersRnd[2]};
        
        rnd = new System.Random();
        GameObject[] emptyANDPassengers = originalArray.OrderBy(e => rnd.NextDouble()).ToArray();
        GameObject[] shuffledArray = new GameObject []{passengersRnd[3], passengersRnd[4], emptyANDPassengers[0], emptyANDPassengers[1], emptyANDPassengers[2], emptyANDPassengers[3]};

        int index = 7;
        // if (user_perspective == "fountain_standing"){
        //     index = 7;
        // } else{
        //      index = 10;
        // }
        if(isPassenger){
            thisCar0 =  createEntity(setorderArr[setorder, index], shuffledArray[0]);
            thisCar1 =  createEntity(setorderArr[setorder, index], shuffledArray[1]);
            thisCar2 =  createEntity(setorderArr[setorder, index], shuffledArray[2]);
            thisCar3 =  createEntity(setorderArr[setorder, index], shuffledArray[3]);
            thisCar4 =  createEntity(setorderArr[setorder, index], shuffledArray[4]);
            thisCar5 =  createEntity(setorderArr[setorder, index], shuffledArray[5]);
        } else{
            thisCar0 =  createEntity(setorderArr[setorder, index], carEmpty);
            thisCar1 =  createEntity(setorderArr[setorder, index], carEmpty);
            thisCar2 =  createEntity(setorderArr[setorder, index], carEmpty);
            thisCar3 =  createEntity(setorderArr[setorder, index], carEmpty);
            thisCar4 =  createEntity(setorderArr[setorder, index], carEmpty);
            thisCar5 =  createEntity(setorderArr[setorder, index], carEmpty);
 
        }

        dstTravelled0 = 15;
        dstTravelled1 = 12;
        dstTravelled2 = 9;
        dstTravelled3 = 6;
        dstTravelled4 = 3;
        dstTravelled5 = 0;

                rnd = new System.Random(); 
                GameObject[] rndPedestrianArr = pedestrianArr.OrderBy(i => rnd.Next()).Take(7).ToArray();
                    
                    thisPerson3 = createEntity(setorderArr[setorder, 4], rndPedestrianArr[0],  isPerson:true);

                    thisPerson4 = createEntity(setorderArr[setorder, 2], rndPedestrianArr[1], isPerson:true);

                    thisPerson1 = createEntity(setorderArr[setorder, 3], rndPedestrianArr[3], isPerson:true);
                    thisPerson6 = createEntity(setorderArr[setorder, 15], rndPedestrianArr[4],  isPerson:true);
                    thisPerson9 = createEntity(setorderArr[setorder, 3], rndPedestrianArr[5],  isPerson:true);
                    thisPerson5 = createEntity(setorderArr[setorder, 9], rndPedestrianArr[2],  isPerson:true);
       //    thisPerson8 = createEntity(setorderArr[setorder, 11], rndPedestrianArr[1],  isPerson:true);
       //    thisPerson0 = createEntity(setorderArr[setorder, 12], rndPedestrianArr[2],  isPerson:true);
       //    thisPerson2 = createEntity(setorderArr[setorder, 6], rndPedestrianArr[4],  isPerson:true);
       //    thisPerson7 = createEntity(setorderArr[setorder, 6], rndPedestrianArr[4],  isPerson:true);


            dstTravelledPerson0 = 4;
                dstTravelledPerson1 = 0;
                dstTravelledPerson2 = 14;
                dstTravelledPerson3 = 3;
                dstTravelledPerson4 = 10;
                dstTravelledPerson5 = 2;
                dstTravelledPerson6 = 4; //12;//
                dstTravelledPerson7 = 6;
                dstTravelledPerson8 = 4;
                dstTravelledPerson9 = 4;

                // thisPerson1 = createEntity(setorderArr[setorder, 3], rndPedestrianArr[1], isPerson:true);
                // // thisPerson3 = createEntity(setorderArr[setorder, 4], rndPedestrianArr[3],  isPerson:true);
                // // thisPerson4 = createEntity(setorderArr[setorder, 2], rndPedestrianArr[4], isPerson:true);
                // thisPerson5 = createEntity(setorderArr[setorder, 14], rndPedestrianArr[0],  isPerson:true);

                // // thisPerson9 = createEntity(setorderArr[setorder, 8], rndPedestrianArr[5], isPerson:true);
                // // if (user_perspective == "fountain_standing"){
                //     thisPerson0 = createEntity(setorderArr[setorder, 12], rndPedestrianArr[2],  isPerson:true);
                //     // thisPerson3 = createEntity(setorderArr[setorder, 4], rndPedestrianArr[3],  isPerson:true);
                //     thisPerson8 = createEntity(setorderArr[setorder, 11], rndPedestrianArr[4],  isPerson:true);
                //     thisPerson4 = createEntity(setorderArr[setorder, 13], rndPedestrianArr[5],  isPerson:true);
                //     thisPerson2 = createEntity(setorderArr[setorder, 6], rndPedestrianArr[3],  isPerson:true);
                // // } else{
                //     thisPerson6 = createEntity(setorderArr[setorder, 5], rndPedestrianArr[3],  isPerson:true);
                //     thisPerson8 = createEntity(setorderArr[setorder, 3], rndPedestrianArr[5],  isPerson:true);
                //     thisPerson7 = createEntity(setorderArr[setorder, 6], rndPedestrianArr[4],  isPerson:true);
                //     thisPerson2 = createEntity(setorderArr[setorder, 6], rndPedestrianArr[2],  isPerson:true);
                // // }
        
                // dstTravelledPerson0 = 0;
                // dstTravelledPerson1 = 0;
                // dstTravelledPerson2 = 12;
                // dstTravelledPerson3 = 0;
                // dstTravelledPerson4 = 0;
                // dstTravelledPerson5 = 0;
                // dstTravelledPerson6 = 5;
                // dstTravelledPerson7 = 4;
                // dstTravelledPerson8 = 4;
                // dstTravelledPerson9 = 4;
            // }
    } else{ //SIX POD SCATTERED

            if(isPassenger){    

                rnd = new System.Random(); 
                GameObject[] myRndNos = passengerArr.OrderBy(i => rnd.Next()).Take(6).ToArray();
                GameObject [] originalArray = new GameObject []{carEmpty, carEmpty, myRndNos[0], myRndNos[1], myRndNos[2], myRndNos[3]};
                rnd = new System.Random();
                GameObject[] shuffledArray = originalArray.OrderBy(e => rnd.NextDouble()).ToArray();
                if (user_perspective == "fountain_standing"){
                    thisCar2 =  createEntity(setorderArr[setorder, 2], shuffledArray[2]);
                    thisCar3 =  createEntity(setorderArr[setorder, 3], shuffledArray[3]);
                }else{
                    thisCar5 =  createEntity(setorderArr[setorder, 6], shuffledArray[5]);
                    thisCar7 =  createEntity(setorderArr[setorder, 2], myRndNos[5]);
                }
                thisCar0 =  createEntity(setorderArr[setorder, 0], shuffledArray[0]);
                thisCar1 =  createEntity(setorderArr[setorder, 18], shuffledArray[1]);


                thisCar4 =  createEntity(setorderArr[setorder, 4], shuffledArray[4]);
                thisCar6 =  createEntity(setorderArr[setorder, 5], myRndNos[4]);


            }else{
                if (user_perspective == "fountain_standing"){
                    thisCar2 =  createEntity(setorderArr[setorder, 2], carEmpty);
                    thisCar3 =  createEntity(setorderArr[setorder, 3], carEmpty);
                }else{
                    thisCar5 =  createEntity(setorderArr[setorder, 6], carEmpty);
                    thisCar7 =  createEntity(setorderArr[setorder, 2], carEmpty);
                }

                thisCar0 =  createEntity(setorderArr[setorder, 0], carEmpty);
                thisCar1 =  createEntity(setorderArr[setorder, 18], carEmpty);

                thisCar4 =  createEntity(setorderArr[setorder, 4], carEmpty);
                thisCar6 =  createEntity(setorderArr[setorder, 5], carEmpty);
            }

                dstTravelled0 = 0;
                dstTravelled1 = 9;
                dstTravelled2 = 0;
                dstTravelled3 = 0;
                dstTravelled4 = 3;
                dstTravelled5 = 10;
                dstTravelled6 = 9;
                dstTravelled7 = 2;

                rnd = new System.Random(); 
                GameObject[] rndPedestrianArr = pedestrianArr.OrderBy(i => rnd.Next()).Take(7).ToArray();

                thisPerson1 = createEntity(setorderArr[setorder, 3], rndPedestrianArr[0], isPerson:true);
                thisPerson2 = createEntity(setorderArr[setorder, 6], rndPedestrianArr[1],  isPerson:true);

                thisPerson8 = createEntity(setorderArr[setorder, 3], rndPedestrianArr[2],  isPerson:true);

                if (user_perspective == "fountain_standing"){
                    // thisPerson0 = createEntity(setorderArr[setorder, 2], rndPedestrianArr[0],  isPerson:true);
                    thisPerson4 = createEntity(setorderArr[setorder, 17], rndPedestrianArr[3], isPerson:true);
                    thisPerson5 = createEntity(setorderArr[setorder, 16], rndPedestrianArr[4],  isPerson:true);
                    thisPerson6 = createEntity(setorderArr[setorder, 19], rndPedestrianArr[5],  isPerson:true);
                    thisPerson7 = createEntity(setorderArr[setorder, 6], rndPedestrianArr[6],  isPerson:true);
                    // thisPerson3 = createEntity(setorderArr[setorder, 9], rndPedestrianArr[0],  isPerson:true);
                    // thisPerson9 = createEntity(setorderArr[setorder, 8], rndPedestrianArr[5], isPerson:true);
                }else{
                    thisPerson4 = createEntity(setorderArr[setorder, 17], rndPedestrianArr[3], isPerson:true);
                    // thisPerson5 = createEntity(setorderArr[setorder, 9], rndPedestrianArr[0],  isPerson:true);
                    thisPerson6 = createEntity(setorderArr[setorder, 19], rndPedestrianArr[4],  isPerson:true);
                    thisPerson7 = createEntity(setorderArr[setorder, 6], rndPedestrianArr[5],  isPerson:true);

                    thisPerson3 = createEntity(setorderArr[setorder, 10], rndPedestrianArr[6],  isPerson:true);
                //                     thisPerson0 = createEntity(setorderArr[setorder, 2], rndPedestrianArr[0],  isPerson:true);
                // thisPerson4 = createEntity(setorderArr[setorder, 2], rndPedestrianArr[4], isPerson:true);

                // thisPerson6 = createEntity(setorderArr[setorder, 5], rndPedestrianArr[5],  isPerson:true);
                // thisPerson7 = createEntity(setorderArr[setorder, 6], rndPedestrianArr[4],  isPerson:true);
                // thisPerson9 = createEntity(setorderArr[setorder, 8], rndPedestrianArr[5], isPerson:true);
                }


        
                dstTravelledPerson0 = 6;
                dstTravelledPerson1 = 9;
                dstTravelledPerson2 = 20;
                dstTravelledPerson3 = 13; //7
                dstTravelledPerson4 = 0;
                dstTravelledPerson5 = 0;
                dstTravelledPerson6 = 12;//7
                dstTravelledPerson7 = 0;
                dstTravelledPerson8 = 7;
                dstTravelledPerson9 = 0; //TOO CLOSE

            }
}

    void destroyAllEntities(){
        entities = new entityStruct[] {thisCar0, thisCar1, thisCar2, thisCar3, thisCar4, thisCar5, thisCar6, thisCar7, thisPerson0, thisPerson1, thisPerson2, thisPerson3, thisPerson4, thisPerson5, thisPerson6, thisPerson7, thisPerson8, thisPerson9};
        currentEntitiesList.Clear();
        for(int i=0; i < entities.Length; i++){
            if(entities[i].carLocal){ 
                print("destroying:" + entities[i].carLocal.name);
                Destroy(entities[i].carLocal);
                if(entities[i].projecting) {
                    DestroyObjects (entities[i].holderLocal); entities[i].projecting = false;
                }
            }
        }
    }

    void ifCarExistsRunIt(){
            if(thisCar0.carLocal){
                dstTravelled0 = moveEntity(ref thisCar0, dstTravelled0); 
            }

            if(thisCar1.carLocal){
                dstTravelled1 = moveEntity(ref thisCar1, dstTravelled1); 
            }

            if(thisCar2.carLocal){
                dstTravelled2 = moveEntity(ref thisCar2, dstTravelled2); 
            }

            if(thisCar3.carLocal){
                dstTravelled3 = moveEntity(ref thisCar3, dstTravelled3); 
            }
            if(thisCar4.carLocal){
                dstTravelled4 = moveEntity(ref thisCar4, dstTravelled4); 
            }
            if(thisCar5.carLocal){
                dstTravelled5 = moveEntity(ref thisCar5, dstTravelled5); 
            }
            if(thisCar6.carLocal){
                dstTravelled6 = moveEntity(ref thisCar6, dstTravelled6); 
            }
            if(thisCar7.carLocal){
                dstTravelled7 = moveEntity(ref thisCar7, dstTravelled7); 
            }

            if(thisPerson0.carLocal){
                dstTravelledPerson0 = moveEntity(ref thisPerson0, dstTravelledPerson0);
            }
            if(thisPerson1.carLocal){
                dstTravelledPerson1 = moveEntity(ref thisPerson1, dstTravelledPerson1);
            }
            if(thisPerson2.carLocal){
                dstTravelledPerson2 = moveEntity(ref thisPerson2, dstTravelledPerson2);
            }
            if(thisPerson3.carLocal){
                dstTravelledPerson3 = moveEntity(ref thisPerson3, dstTravelledPerson3);
            }
            if(thisPerson4.carLocal){
                dstTravelledPerson4 = moveEntity(ref thisPerson4, dstTravelledPerson4);
            }
            if(thisPerson5.carLocal){
                dstTravelledPerson5 = moveEntity(ref thisPerson5, dstTravelledPerson5);
            }
            if(thisPerson6.carLocal){
                dstTravelledPerson6 = moveEntity(ref thisPerson6, dstTravelledPerson6);
            }
            if(thisPerson7.carLocal){
                dstTravelledPerson7 = moveEntity(ref thisPerson7, dstTravelledPerson7);
            }
            if(thisPerson8.carLocal){
                dstTravelledPerson8 = moveEntity(ref thisPerson8, dstTravelledPerson8);
            }
            if(thisPerson9.carLocal){
                dstTravelledPerson9 = moveEntity(ref thisPerson9, dstTravelledPerson9);
            }
        }

    void singleCarPresets(){
        runCar = false;
        dstTravelled0 = 0;
    }
    entityStruct createEntity(VertexPath pathLocal, GameObject entity, bool projecting = false, bool isPerson = false, float speedLocal = speedAvg, bool repeatMovement = false){
            // runCar = false;
            print("Creating entity: " + entity.name);
            GameObject entityInstantiated = Instantiate(entity, pathLocal.GetPoint(0), pathLocal.GetRotation(0)); //instantiates that projection_stem
            entityStruct entityStructLocal = new entityStruct();
            if(!isPerson){
                GameObject frontwheel = entityInstantiated.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
                Rotator frontwheelScript = frontwheel.GetComponent<Rotator>(); 
                Vector3 frontWheelRot = frontwheelScript._rotation; 

                GameObject backwheel = entityInstantiated.transform.GetChild(0).GetChild(1).gameObject;
                Rotator backwheelScript = backwheel.GetComponent<Rotator>(); 
                Vector3 backWheelRot = backwheelScript._rotation; 
                backwheelScript._rotation.x = speedLocal*120;
                frontwheelScript._rotation.x = speedLocal*240;
            }
            
            if(materialMapping.ContainsKey(entity)){
                    materialCurrent = materialMapping[entity];
                    materialCurrent.color = colorsShirts[colorCount];
                    colorCount++; if(colorCount == colorCountMax) {colorCount = 0;}
                    // print(entity.name + " " + materialCurrent.color + " " + materialCurrent.name);
            }

            entityInstantiated.transform.position = pathLocal.GetPoint(0);
            entityInstantiated.SetActive(true);

            entityStructLocal.carLocal = entityInstantiated;
            if(isPerson == true){
                entityStructLocal.speedLocal = speedSlow;
            }else{
                entityStructLocal.speedLocal = speedLocal;
            }
            entityStructLocal.pathLocal = pathLocal;
            entityStructLocal.numRepeatMovement = repeatMovement;
            // print(entityStructLocal.carLocal + " exists");
            currentEntitiesList.Add(entityStructLocal.carLocal.name);
            // print("added:" + entityStructLocal.carLocal.name);
            entityStructLocal.projecting = projecting;
           // projecting // checking global variable
            if(entityStructLocal.projecting && !isPerson){
                entityStructLocal.projecting = true; //can delete this
                entityStructLocal.holderLocal = new GameObject(); 
            }

            return entityStructLocal;
    }

    float moveEntity(ref entityStruct entity, float dstTravelledLocal = 0){
            //define all parameters
            VertexPath pathLocal = entity.pathLocal; float speedLocal = entity.speedLocal; GameObject carLocal = entity.carLocal;
            //move car along
            dstTravelledLocal += speedLocal * Time.deltaTime;
            // carLocal.transform.position += new Vector3() dstTravelledLocal;
            carLocal.transform.position = pathLocal.GetPointAtDistance(dstTravelledLocal, end);
            carLocal.transform.rotation = pathLocal.GetRotationAtDistance(dstTravelledLocal, end);
            // print(entity.carLocal);
            // // print(entity.carLocal.name);
            // // print(carBryce.name);

            //     Rigidbody rigid = entity.carLocal.GetComponent<Rigidbody>();
            //     if(rigid) print("velocity: " + rigid.velocity);
            //     else print("Bryce but no rigidbody");
   


            if (carLocal.transform.position == pathLocal.GetPoint(pathLocal.NumPoints - 1)){
                if(entity.numRepeatMovement == true){
                    print("repeating movement");
                    entity.numRepeatMovement = false; 
                    dstTravelledLocal = 0;
                }else{
                    // print("before :" + currentEntitiesList.Count + currentEntitiesList.ToString());
                    // print("destroying object: " + carLocal.name);
                    // print("removing:" + entity.carLocal);
                    currentEntitiesList.Remove(entity.carLocal.name);
                    Destroy(carLocal);
                    stopWatch.Stop();
                    TimeSpan ts = stopWatch.Elapsed;

                    // Format and display the TimeSpan value. 
                    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds,ts.Milliseconds / 10);
                    print("STOPWATCH:" + elapsedTime);
                    print("DISTANCE TRAVELLED: " + dstTravelledLocal);
                    print("Set speed: " + speedLocal);
                    // print("after :" + currentEntitiesList.Count + currentEntitiesList.ToString());

                    if(entity.projecting) {DestroyObjects (entity.holderLocal); entity.projecting = false;}
                }
            }

            if(entity.projecting){
                Vector3 point; Quaternion rot; GameObject cube;
                DestroyObjects (entity.holderLocal);

                VertexPath path = pathLocal;

                spacing = Mathf.Max(minSpacing, spacing);
                float dstStartArrow = dstTravelledLocal + 0.85f; //start of arrow
                float dstEndArrow = dstTravelledLocal + 1.8f;

                if(pathLocal.GetPointAtDistance(dstEndArrow, end) == pathLocal.GetPoint(pathLocal.NumPoints - 1)){}
                else{
                while (dstStartArrow < dstEndArrow) {
                    point = path.GetPointAtDistance (dstStartArrow);
                    rot = path.GetRotationAtDistance (dstStartArrow);
                    cube = Instantiate (projection_stem, point, rot);
                    cube.transform.parent = entity.holderLocal.transform;
                    // cube.transform.SetParent(holder.transform);
                    cube.SetActive(true);
                    dstStartArrow += spacing; //keep adding lines until hit end of dstEndArrow
                }

                    point = path.GetPointAtDistance (dstStartArrow);
                    rot = path.GetRotationAtDistance (dstStartArrow);
                    cube = Instantiate (projection_arrow, point, rot);
                    cube.transform.parent = entity.holderLocal.transform;
                    // cube.transform.SetParent(holder.transform);
                    cube.SetActive(true);
                    dstStartArrow += spacing;
                }
            }
            return dstTravelledLocal;
    }

            void DestroyObjects (GameObject holder) {
                int numChildren = holder.transform.childCount;
                for (int i = numChildren - 1; i >= 0; i--) {
                    DestroyImmediate (holder.transform.GetChild (i).gameObject, false);
                }
            
        }

//should be called change to new position in general, also need to change the display of which scenario it is.
    void changePosition(){
        print("IRYNA in CHANGEposition, current scenario: " + scenario_current);
            //start SITTING
            // if(scenario_current >=0 && scenario_current < 10){
            //     user_perspective = "cafe_sitting";
            // }else{
            //     user_perspective = "fountain_standing";
            // }
            // start STANDING

        if(breakON){
                txt.text = ""; 
                if(user_perspective == firstSet){
                    print("in BREAK ON: should be now" + secondSet);
                    user_perspective = secondSet;
                }else{
                    print("in BREAK ON: should be now" + firstSet);
                    user_perspective = firstSet;
                }

                breakON = false;

        }else if(scenario_current >=-2 && scenario_current < 10){
                print(firstSet + scenario_current);
                user_perspective = firstSet; 
            }else{
                print(secondSet + scenario_current);
                user_perspective = secondSet;
            }
        if(user_perspective =="fountain_standing"){
            print("ACTUALLY fountain standing");
            //remove this user perspective
            cameraCafe.transform.position = new Vector3(2.3384766578674318f,-2.558000087738037f,3.428985118865967f);
            cameraCafe.transform.rotation = new Quaternion(0,0.0029249044600874187f,0,0.999995708f);

            //fountain canvas - rename to just canvas
           //displayCafe.transform.position = new Vector3(1.787984848022461f,2.1653332710266115f,4.998656272888184f);
           //displayCafe.transform.rotation = new Quaternion(0, 0, 0, 1.0f);
            headlessCafeGuy.SetActive(false);

            // standingPerson.SetActive(true);
        }else{ //true cameracafe
            print("ACTUALLY cameraCafe");
            cameraCafe.transform.position = new Vector3(10.542476654052735f,-2.7160000801086427f,4.534656524658203f);
            cameraCafe.transform.rotation = new Quaternion(0,-0.707106829f,0,0.707106829f);

           //cafe
            //displayCafe.transform.position = new Vector3(8.497983932495118f,2.1653332710266115f,4.628656387329102f);
            //displayCafe.transform.rotation = new Quaternion(0.0f,-0.7071068286895752f,0.0f, 0.7071068286895752f);
            headlessCafeGuy.SetActive(true);
        }

    }

        void scenarioCounterKeyboard(){
            justChangedScenario = false;
            if(Input.GetKeyDown(KeyCode.RightArrow) || OVRInput.GetDown(OVRInput.Button.One) || OVRInput.GetDown(OVRInput.Button.Three)){
                Debug.Log(kTAG2 + " OVRInput.Button.One / right key pressed");
                //increment next;
                breakON = false;
                scenario_current++;
                if(scenario_current > scenario_current_max){
                    scenario_current = scenario_current_max;
                }
                activeScreen = true;
                Debug.Log(kTAG2 + "scenario # " + scenario_current);
            } 

            // returns true if the secondary button (typically "B") was pressed this frame on either controller
            else if(Input.GetKeyDown(KeyCode.LeftArrow) || OVRInput.GetDown(OVRInput.Button.Two) || OVRInput.GetDown(OVRInput.Button.Four)){
                Debug.Log(kTAG2 + " OVRInput.Button.Two / left key pressed");
                //go back a scenario
                breakON = false;
                scenario_current--;
                if(scenario_current < 0){
                    scenario_current = 0 ;
                }
                activeScreen = true;
                Debug.Log(kTAG2 + " scenario # " + scenario_current);

            }else if(Input.GetKeyDown(KeyCode.DownArrow) || OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) || OVRInput.Get(OVRInput.RawButton.RIndexTrigger)|| OVRInput.Get(OVRInput.RawButton.LIndexTrigger) || OVRInput.Get(OVRInput.Button.PrimaryThumbstick)){
                Debug.Log(kTAG2 + " down key pressed");
                //reset to nothing, just sitting;
                breakON = true;
                activeScreen = true;
                Debug.Log(kTAG2 + " scenario # " + scenario_current);
            }

    } 
    
void displayText(){
    if(!runCar && currentEntitiesList.Count == 0){
        // print("runcar is false and 0 entities present");
        timer.stopbutton();
        txt.text = "";
        displayCafe.SetActive(true);

        // displayCafe.transform.GetChild(0).
    }

    
    if(activeScreen){
        if(scenario_current == -1){
           txt.text = ""; 
        // }else if (scenario_current == 0){
        //     txt.text = "warmup";
        }else{
            int newNum = scenario_current + 1;
            txt.text = "Scenario #" + newNum.ToString();
        }

        displayCafe.SetActive(true);
        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            displayCafe.SetActive(false);
            activeScreen = false;
            currentTime = TimeAmount;
        }
    }
}

void getDisplay(){
        //got text of the Canvas GameObject (but not using it because it's too close to user)
        // txt = display.GetComponent<Text>();
        // print("here1 " + txt.text);

        //got second child of Canvas, which is Text
        GameObject x = displayCafe.transform.GetChild(1).gameObject;
        txt = x.GetComponent<Text>();
        print("got Display: " + txt);
}
}

// }


        // if((scenario_current == 1) && runCar){
        //     runCar = false;
        //     thisCar0 =  createEntity(path1, carEmpty); //pathPlatoon not bad
        //     thisCar1 =  createEntity(path4, carElizabeth);
        //     thisCar2 =  createEntity(path5, carBryce);
        //     thisPerson0 = createEntity(path2, Megan, true);
        //     thisPerson1 = createEntity(path3, Josh, true);
        //     thisPerson2 = createEntity(path6, Lewis, true);

            
        //     // currentEntitiesList = new List<entityStruct>{thisCar0, thisCar1, thisCar2, thisPerson0, thisPerson1, thisPerson2};
        //     dstTravelled0 = 1;
        //     dstTravelled1 = 0;
        //     dstTravelled2 = 0;

        //     dstTravelledPerson0 = 4;
        //     dstTravelledPerson1 = 6;
        //     dstTravelledPerson2 = 8;
        // }

        // if((scenario_current == 2) && runCar){
        //     runCar = false;
        //     thisCar0 =  createEntity(path1, carEmpty); //pathPlatoon not bad
        //     thisCar1 =  createEntity(path4, carEmpty);
        //     thisCar2 =  createEntity(path5, carBryce);
        //     thisCar4 =  createEntity(path2, carElizabeth);
        //     thisCar5 =  createEntity(path3, carEmpty);
        //     thisCar6 =  createEntity(pathPlatoon, carEmpty);
             
             
        //     thisPerson0 = createEntity(path2, Megan, true);
        //     thisPerson1 = createEntity(path3, Josh, true);
        //     thisPerson2 = createEntity(path6, Lewis, true);
        //     dstTravelled4 = 8;
        //     dstTravelled5 = 12;
        //     dstTravelled6 = 6;

        //     thisCar0.projecting = true; //can delete this
        //     thisCar1.projecting = true;
        //     thisCar2.projecting = true;

        //     thisCar0.holderLocal = new GameObject(); //match 0 and not holder1
        //     thisCar1.holderLocal = new GameObject();
        //     thisCar2.holderLocal = new GameObject();

        //     // thisCar0.numRepeatMovement = true;

        //     //can we create object holders on the spot?

        //     dstTravelled0 = 1;
        //     dstTravelled1 = 0;
        //     dstTravelled2 = 0;
        //             dstTravelledPerson0 = 4;
        //     dstTravelledPerson1 = 6;
        //     dstTravelledPerson2 = 8;
        // }
  // if((scenario_current == 1) && runCar){
        //     singleCarPresets();
        //     thisCar0 = createEntity(path0, carEmpty);
        // }

        // if((scenario_current == 2) && runCar){
        //     singleCarPresets();
        //     thisCar0 = createEntity(path0, carBryce);
        // }

        
        // if((scenario_current == 3) && runCar){
        //     singleCarPresets();
        //     thisCar0 = createEntity(path0, carBryce, speedLocal:speedSlow);
        // }

        
        // if((scenario_current == 4) && runCar){
        //     singleCarPresets();
        //     thisCar0 = createEntity(path0, carBryce, speedLocal:speedFast);
        // }

        // if((scenario_current == 5 || scenario_current == last_scenario_Cafe+1) && runCar){
        //     runCar = false;
        //     // thisCar0 =  createEntity(pathPlatoon, carEmpty);
        //     // thisCar1 =  createEntity(pathPlatoon, carEmpty);
        //     // thisCar2 =  createEntity(pathPlatoon, carElizabeth);
        //     // thisCar3 =  createEntity(pathPlatoon, carEmpty);
        //     thisCar4 =  createEntity(pathPlatoon, carEmpty);
        //     thisCar5 =  createEntity(pathPlatoon, carElizabeth);
        //     thisCar6 =  createEntity(pathPlatoon, carEmpty);
            
        //     // dstTravelled0 = 18;
        //     // dstTravelled1 = 15;
        //     // dstTravelled2 = 12;
        //     // dstTravelled3 = 9;
        //     dstTravelled4 = 6;
        //     dstTravelled5 = 3;
        //     dstTravelled6 = 0;
        // }

        // if((scenario_current == 6 || scenario_current == last_scenario_Cafe+2) && runCar){
        //     runCar = false;
        //     thisCar0 =  createEntity(path1, carEmpty); //pathPlatoon not bad
        //     thisCar1 =  createEntity(path4, carEmpty);
        //     thisCar2 =  createEntity(path5, carElizabeth);
            
        //     dstTravelled0 = 1;
        //     dstTravelled1 = 0;
        //     dstTravelled2 = 0;
        // }

        // if((scenario_current == 7 || scenario_current == last_scenario_Cafe+3) && runCar){
        //     runCar = false;
        //     thisCar0 =  createEntity(path1, carEmpty); //pathPlatoon not bad
        //     thisCar1 =  createEntity(path4, carEmpty);
        //     thisCar2 =  createEntity(path5, carBryce);

        //     thisCar0.projecting = true; //can delete this
        //     thisCar1.projecting = true;
        //     thisCar2.projecting = true;

        //     thisCar0.holderLocal = new GameObject(); //match 0 and not holder1
        //     thisCar1.holderLocal = new GameObject();
        //     thisCar2.holderLocal = new GameObject();

        //     // thisCar0.numRepeatMovement = true;

        //     //can we create object holders on the spot?

        //     dstTravelled0 = 1;
        //     dstTravelled1 = 0;
        //     dstTravelled2 = 0;
        // }

        // if((scenario_current == 8 || scenario_current == last_scenario_Cafe+4) && runCar){
        //     runCar = false;
        //     thisCar0 =  createEntity(path1, carEmpty); //pathPlatoon not bad
        //     thisCar1 =  createEntity(path4, carEmpty);
        //     thisCar2 =  createEntity(path5, carBryce);
        //     thisPerson0 = createEntity(path2, Megan, true);
        //     thisPerson1 = createEntity(path3, Josh, true);
        //     thisPerson2 = createEntity(path6, Lewis, true);
        //     // currentEntitiesList = new List<entityStruct>{thisCar0, thisCar1, thisCar2, thisPerson0, thisPerson1, thisPerson2};
        //     dstTravelled0 = 1;
        //     dstTravelled1 = 0;
        //     dstTravelled2 = 0;

        //     dstTravelledPerson0 = 4;
        //     dstTravelledPerson1 = 6;
        //     dstTravelledPerson2 = 8;
        // }


//to change speeds, I need to figure out how to change their walking
        // if((scenario_current == 0 || scenario_current == last_scenario_Cafe+5) && runCar){
        //     runCar = false;
        //     thisCar0 =  createEntity(speedSlow, path1, carEmpty); //pathPlatoon not bad
        //     thisCar1 =  createEntity(speedFast, path4, carEmpty);
        //     thisCar2 =  createEntity(speedAvg, path5, carPerson);
        //     thisPerson0 = createEntity(speedSlow, path2, Megan, true);
        //     thisPerson1 = createEntity(speedFast, path3, Josh, true);
        //     thisPerson2 = createEntity(speedAvg, path6, Lewis, true);
            
        //     dstTravelled0 = 1;
        //     dstTravelled1 = 0;
        //     dstTravelled2 = 0;
        // }


        // if((scenario_current == 9 || scenario_current == last_scenario_Cafe+5) && runCar){
        //     runCar = false;
        //     thisCar0 =  createEntity(path0, carEmpty);
        //     thisCar1 =  createEntity(path1, carElizabeth);
        //     thisCar2 =  createEntity(path2, carBryce);
        //     thisCar3 =  createEntity(path3, carEmpty);
        //     thisCar4 =  createEntity(path4, carEmpty);
        //     thisCar5 =  createEntity(path5, carEmpty);
        //     // thisCar6 =  createEntity(path6, carBryce);
            
        //     dstTravelled0 = 0;
        //     dstTravelled1 = 0;
        //     dstTravelled2 = 0;
        //     dstTravelled3 = 0;
        //     dstTravelled4 = 0;
        //     dstTravelled5 = 0;
            // dstTravelled6 = 0;
        // }
        // last_scenario_Cafe = 9;

///////////////////////////////////////////////////////////////////////////////////////////this goes above start
///
// public List<Transform> childsBryce  = new List<Transform>();
// public List<Transform> childsElizabeth  = new List<Transform>();
///////////////////////////////////////////////////////////////////////////////////////////THIS GOES IN THE START
///    //    FindEveryChild(Bryce.transform, childsBryce);
    //     for (int i = 0; i < childsBryce.Count; i++)
    //     {
    //         FindEveryChild(childsBryce[i], childsBryce);
    //     }

    //     FindEveryChild(Passenger.transform, childsElizabeth);
    //     for (int i = 0; i < childsElizabeth.Count; i++)
    //     {
    //         FindEveryChild(childsElizabeth[i], childsElizabeth);
    //     }

    //     print(childsBryce[2]);
    //     print(childsBryce[4]);
    //     print(childsElizabeth[2]);
    //     print(childsElizabeth[4]);

    //     for (int i = 0; i < childsBryce.Count; i++)
    //     {
    //         print(childsBryce[i]);
    //         print(childsElizabeth[i]);
    //         childsElizabeth[i].transform.position = childsBryce[i].transform.position; 
    //         childsElizabeth[i].transform.rotation = childsBryce[i].transform.rotation;
    //     }


    /////////////////////////////////////////////////////////////////////////////HIDING CONTROLLERS - I don't think this code worked
    ///        //Hide controllers
        // print(leftController.GetComponent<MeshRenderer>());
        // leftController.GetComponent<MeshRenderer>().enabled = false;
        // rightController.GetComponent<MeshRenderer>().enabled = false;
        // leftController.transform.localScale = new Vector3(0, 0, 0);
        // rightController.transform.localScale = new Vector3(0, 0, 0);

        //OR // Hide button https://discussions.unity.com/t/how-to-make-an-gameobject-invisible-and-disappeared/174
       // GameObject.Find ("LeftController").transform.localScale = new Vector3(0, 0, 0);