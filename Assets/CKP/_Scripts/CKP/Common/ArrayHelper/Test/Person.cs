using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ArrayHelperTest
{
    public class Person : MonoBehaviour, IComparable<Person>
    {
        public string id;
        public int age;
        public float height;

        public static bool AgeEqualFifty(Person person)
        {
            return person.age == 50;

        }

        public static bool AgeGreaterThanTwenty(Person person)
        {
            return person.age > 20;
        }

        public int CompareTo(Person other)
        {
            if (age < other.age)
            {
                return 1;
            }
            else if (age > other.age)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
