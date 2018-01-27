using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public int numberOfDebris;
    public float maxPower;
    private GameObject[] debrisList;
    private float randX;
    private float randY;
    private GameObject obj;

    void Start ()
    {
        debrisList = Resources.LoadAll<GameObject>("Crate_debris");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Explode(transform);
        }
    }

    public void Explode(Transform currentTransform)
    {
        for (int i = 0; i < numberOfDebris; i++)
        {
            obj = Instantiate(debrisList[Random.Range(0, debrisList.Length)], currentTransform.position, currentTransform.rotation);
        }
        StartCoroutine(RollNew());
    }

    IEnumerator RollNew()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, 1, LayerMask.NameToLayer("Debris"));
        for (int i = 0; i < objects.Length; i++)
        {
            GameObject newObj = objects[i].gameObject;
            randX = Random.value * Random.Range(-maxPower, maxPower);
            randY = Random.value * Random.Range(-maxPower, maxPower);
            GetComponent<Rigidbody2D>().AddForce(new Vector2(randX, randY));
        }
        
        yield return new WaitForEndOfFrame();
    }
}
