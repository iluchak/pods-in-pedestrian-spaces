using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackUpCode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //INSIDE if this car
            //DIDNT work to rotate wheel left and right
            // Quaternion yes = frontwheelOverall.transform.rotation;
            // yes.z = pathCreator.path.GetRotationAtDistance(dstTravelled, end).z;
            // // yes.z = pathCreator.path.GetRotationAtDistance(dstTravelled, end).z;

            // //you can minus add stuff. so the difference between y and z from last time
            // frontwheelOverall.transform.rotation = yes;
// wheel rotated left and then right quaternion
// Quaternion(0.971671581,-0.0107700238,0.0451595895,0.231730595) -
// Quaternion(0.956764936,0.0418445989,-0.175459296,0.228175536)


/////ORIGINAL createObject

            // thisCar = Instantiate(carEmpty, pathCreator.path.GetPoint(0),pathCreator.path.GetRotation(0)); //instantiates that prefab
            // // frontwheelOverall = carEmpty.transform.GetChild(0).GetChild(0).gameObject;
            // // print(frontwheelOverall.name);
            // GameObject frontwheel = thisCar.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
            // Rotator frontwheelScript = frontwheel.GetComponent<Rotator>(); //it recognizes this class so how do I get access to its public variables?
            // Vector3 frontWheelRot = frontwheelScript._rotation; //IT"S A THING! i got it!

            // GameObject backwheel = thisCar.transform.GetChild(0).GetChild(1).gameObject;
            // Rotator backwheelScript = backwheel.GetComponent<Rotator>(); //it recognizes this class so how do I get access to its public variables?
            // Vector3 backWheelRot = backwheelScript._rotation; //IT"S A THING! i got it!
            // backwheelScript._rotation.x = speed*120;
            // frontwheelScript._rotation.x = speed*240;

            // //here maybe I should return the car, the frontwheelspeed, backwheelspeed? do it is it's own method
            // //and things you can pass in (distance travelled)?

            // //next figure out how a good ratio of speed to wheel speed
            // dstTravelled = 0;
            // thisCar.transform.position = pathCreator.path.GetPoint(0);
            // thisCar.SetActive(true);

/////////Marcus original method

    // void runCarMethod(){
    //     //StartCoroutine(runSingleCar());
    //     runSingleCar();
    // }

    // void runSingleCar(){
    //     Debug.Log("creating a new car");
    //     GameObject thisCar = Instantiate(car, pathCreator.path.GetPoint(0),pathCreator.path.GetRotation(0)); //instantiates that prefab
    //     float dstTravelled = 0;
    //     thisCar.transform.position = pathCreator.path.GetPoint(0);
    //     thisCar.SetActive(true);
    //     // Debug.Log("iroeputi");
    //     // while(thisCar){
    //         Debug.Log("inside while");
    //         dstTravelled += speed * 0.05f;//Time.deltaTime;
    //         thisCar.transform.position = pathCreator.path.GetPointAtDistance(dstTravelled, end);
    //         thisCar.transform.rotation = pathCreator.path.GetRotationAtDistance(dstTravelled, end);

    //         // Destroy car when it  reaches the end of the path
    //         if (thisCar.transform.position == pathCreator.path.GetPoint(pathCreator.path.NumPoints - 1)){

    //             Destroy(thisCar);
    //         //}

    //         // yield return new WaitForSeconds(0.05f);
    //     }

    // }


    //THIS was in the start to copy exact body position of Bryce to Elizabeth

// public List<Transform> childsBryce  = new List<Transform>();
// public List<Transform> childsElizabeth  = new List<Transform>();
    
        // FindEveryChild(Bryce.transform, childsBryce);
        // for (int i = 0; i < childsBryce.Count; i++)
        // {
        //     FindEveryChild(childsBryce[i], childsBryce);
        // }

        // FindEveryChild(Elizabeth.transform, childsElizabeth);
        // for (int i = 0; i < childsElizabeth.Count; i++)
        // {
        //     FindEveryChild(childsElizabeth[i], childsElizabeth);
        // }

        // print(childsBryce[2]);
        // print(childsBryce[4]);
        // print(childsElizabeth[2]);
        // print(childsElizabeth[4]);

        // for (int i = 0; i < childsBryce.Count; i++)
        // {
        //     print(childsBryce[i]);
        //     print(childsElizabeth[i]);
        //     childsElizabeth[i].transform.position = childsBryce[i].transform.position; 
        //     childsElizabeth[i].transform.rotation = childsBryce[i].transform.rotation;
        // }

// public void FindEveryChild(Transform parent, List<Transform> childs)
//     {
//         int count = parent.childCount;
//         for (int i = 0; i < count; i++)
//         {
//             childs.Add(parent.GetChild(i));
//         }
//     }

    //in Car struct if projecting , this is the original code but i changed it

                // if (pathCreator0 != null && prefab != null && holder != null) {
            //     DestroyObjects ();

            //     VertexPath path = pathCreator0.path;

            //     spacing = Mathf.Max(minSpacing, spacing);
            //     float dst = dstTravelledLocal+0.85f;

            //     while (dst < (dstTravelledLocal + 2.2f)) {
            //         Vector3 point = path.GetPointAtDistance (dst);
            //         Quaternion rot = path.GetRotationAtDistance (dst);
            //         GameObject cube = Instantiate (prefab, point, rot);
            //         cube.transform.parent = holder.transform;
            //         // cube.transform.SetParent(holder.transform);
            //         // cube.SetActive(true);
            //         dst += spacing;
            //     }
            // }
    

    //CHANGING SHIRT COLOR
            //  //if walking people
        //     GameObject personClothing = cartoInstantiate.transform.GetChild(0).gameObject;
        //     SkinnedMeshRenderer personClothingMesh = personClothing.GetComponent<SkinnedMeshRenderer>();

        //     mats = personClothingMesh.materials;
        //     mats[0] = materialCurrent;
        //     mats[0].color = colorsShirts[colorCount];
        //         //             print("changed to paint2" + colorsShirts[colorCount]);
        //         // print("actual color" + paint2.color + materialShirt.color);
        //     colorCount++;
        //     personClothingMesh.materials = mats;

            
//if Josh
            // print(personClothing.name);
            // if(personClothing.transform.childCount > 0){
            //     personClothingMesh = personClothing.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
            //     mats = personClothingMesh.materials;
            //     mats[0] = materialCurrent; //do we assign it new material? or we assume (change it in th people)
            //     mats[0].color = colorsShirts[colorCount];
            //     //                 print("changed to paint 2" + colorsShirts[colorCount]);
            //     // print("actual color" + paint2.color + materialShirt.color);
            //     colorCount++;
            //     personClothingMesh.materials = mats;

            // }


                ///add like 10 colors
                ///change default materials on people for shirts but create copies first
                //////change material of all shirts to be some random color (but not sure if I need to instantiate new ones or create copies and put them in Resources folder)
                /////figure out how to get the meshRenderer from each person (similar way to how I got the wheel- check code below)
                // Material[] mats = renderer.materials;
                // mats[0] = materialShirt;
                // mats[0].color = colorsShirts[colorCount];
                // renderer.materials = mats;


//if vehicle     Material[] mats; Material material;
                    // GameObject personClothing = cartoInstantiate.transform.GetChild(2).GetChild(1).gameObject;
                    // print(personClothing.name);
                    // SkinnedMeshRenderer personClothingMesh = personClothing.GetComponent<SkinnedMeshRenderer>();
                    // mats = personClothingMesh.materials;
                    // mats[0] = materialCurrent;
                    // mats[0].color = colorsShirts[colorCount];

                    // colorCount++;
                    // personClothingMesh.materials = mats;
}
