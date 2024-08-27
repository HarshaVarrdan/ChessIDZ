using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{

    public static TestScript instance;

    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        printfunc(5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void poly(int a)
    {
        Debug.Log(a);
    }

    void printfunc(int n)
    {

        int x = 1;
        for(int i = 1; i <= n; i++)
        {
            if(i != 1)
            {
                x = x * 10;
            }
            string temp = x.ToString();
            while(temp.Length != n)
            {
                if(temp.Length != n)
                {
                    temp = "0" + temp;
                }
            }
            Debug.Log(temp);
            
        }
    }

}
