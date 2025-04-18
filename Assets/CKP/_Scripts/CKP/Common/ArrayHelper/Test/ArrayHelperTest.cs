using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using System;

namespace ArrayHelperTest
{
    public class ArrayHelperTest : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            Person[] people = new Person[] { new Person() { id = "1", age = 20, height = 168f },
                new Person() { id = "2", age = 51, height = 170f } ,
                new Person() { id = "3", age = 50, height = 171f },
                 new Person() { id = "4", age = 19, height = 160f} };
            Person pAge = people.Find(Person.AgeEqualFifty);
            Debug.Log(pAge.id);
            Person[] resPeople = people.FindAll((e) => { return e.age > 50; });
            foreach (Person item in resPeople)
            { 
                Debug.Log("年龄大于20的人的ID为：" + item.id);
            }

          Person maxAge=people.FindMax((resP,p1)=> { return resP.age>p1.age ? resP : p1; });
            Debug.Log("年龄最大的ID为："+maxAge.id);

            Person maxH = people.GetMax((p) => p.height);
            Debug.Log("身高最大的ID为：" + maxH.id);

            people.AscendingOrder((p)=>p.age);

            //ArrayHelper.SortArray<Person>(ref people);
            foreach (var item in people)
            {
                Debug.Log(item.id + ",");
            } 
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Person");
            Person[] ps= gameObjects.Select<GameObject,Person>((o)=>o.GetComponent<Person>());
            foreach (var item in ps)
            {
                Debug.Log(item.id);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }


}
