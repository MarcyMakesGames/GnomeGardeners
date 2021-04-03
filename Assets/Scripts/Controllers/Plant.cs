using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour, IInteractable, IHeldItem
{
    public int growthStage = 0;
    [SerializeField]
    private List<float> stageTimes;
    [SerializeField]
    private List<GameObject> harvests;
    [SerializeField]
    private List<Sprite> stageSprites;
    private float moisture = 0f;
    private float growTime = 0f;
    private bool isOnArableGround = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Grow()
    {
        // todo: called every time the plant advances stages
    }

    public void Interact()
    {
        // todo: do behaviour based on tool
    }

    public void DropItem(Vector3 position)
    {
        // todo: drop plant on ground; if plantbed then it grows
    }
}
