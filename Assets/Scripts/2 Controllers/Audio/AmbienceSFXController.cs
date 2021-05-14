using UnityEngine;

public class AmbienceSFXController : MonoBehaviour
{
    [Header("Designers")]
    public SoundType ambience;
    public SoundType[] accents;
    public float timeBetweenAccents;
    public float timeVariance;
    [Header("Programmers")]
    public AudioSource accentSource;

    private float timeToNextAccent;
    private float rotationSpeed;

    private AudioManager audioManager;
    private GameTime gameTime;
    private Camera mainCamera;

    private void Start()
    {
        audioManager = GameManager.Instance.AudioManager;
        gameTime = GameManager.Instance.Time;
        audioManager.PlayAmbience(ambience);
        timeToNextAccent = timeBetweenAccents;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rotationSpeed = 0.01f;
    }

    private void Update()
    {
        transform.position = mainCamera.transform.position;
        transform.rotation.eulerAngles.Set(
            transform.rotation.eulerAngles.x, 
            transform.rotation.eulerAngles.y, 
            transform.rotation.eulerAngles.z + rotationSpeed);

        timeToNextAccent -= Time.deltaTime;
        if(timeToNextAccent <= 0f)
        {
            int randomIndex = Random.Range(0, accents.Length);
            var randomAccent = accents[randomIndex];
            audioManager.PlayAmbience(randomAccent, accentSource);
            timeToNextAccent = timeBetweenAccents + Random.Range(-timeVariance, timeVariance);
        }
    }
}
