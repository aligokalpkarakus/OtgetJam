using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageCollector : MonoBehaviour
{
    [Header("Chain Settings")]
    public Transform chainSpawnPoint;
    public float chainSpeed = 5f;
    public float chainReturnSpeed = 8f;
    public float collectionDelay = 4f;

    [Header("Garbage Settings")]
    public List<GameObject> garbageObjects = new List<GameObject>(); // Editor �zerinden ��pleri buraya s�r�kleyebilirsiniz

    private bool isChainActive = false;
    private GameObject currentGarbage;

    // Line Renderer bile�eni zinciri g�rselle�tirmek i�in
    private LineRenderer chainVisual;

    private int currentGarbageIndex = 0;

    void Start()
    {
        // Line Renderer component'ini al veya olu�tur
        chainVisual = GetComponent<LineRenderer>();
        if (!chainVisual)
        {
            chainVisual = gameObject.AddComponent<LineRenderer>();
        }

        // Line Renderer ba�lang�� ayarlar�
        SetupLineRenderer();

        // Zincirin ba�lang�� pozisyonunu ayarla
        chainVisual.positionCount = 2;
        chainVisual.SetPosition(0, chainSpawnPoint.position);
        chainVisual.SetPosition(1, chainSpawnPoint.position);
    }

    void SetupLineRenderer()
    {
        // Temel ayarlar
        chainVisual.startWidth = 0.1f;  // Zincirin ba�lang�� geni�li�i
        chainVisual.endWidth = 0.1f;    // Zincirin biti� geni�li�i

        // Renk ayarlar�
        chainVisual.startColor = Color.gray;  // Zincirin ba�lang�� rengi
        chainVisual.endColor = Color.gray;    // Zincirin biti� rengi
    }

    void Update()
    {
        // E�er zincir aktif de�ilse ve toplanacak ��p varsa, bir sonraki ��p� toplamaya ba�la
        if (!isChainActive && currentGarbageIndex < garbageObjects.Count)
        {
            StartCoroutine(CollectGarbage());
        }
    }

    // D��ar�dan ��pleri ayarlamak i�in (runtime s�ras�nda)
    public void SetGarbageList(List<GameObject> garbages)
    {
        garbageObjects = new List<GameObject>(garbages);
        currentGarbageIndex = 0;

        // �nceki toplama i�lemi varsa durdur
        StopAllCoroutines();
        isChainActive = false;

        // Zinciri ba�lang�� pozisyonuna getir
        chainVisual.SetPosition(0, chainSpawnPoint.position);
        chainVisual.SetPosition(1, chainSpawnPoint.position);
    }

    // Toplama i�lemini manuel olarak ba�latmak i�in
    public void StartCollection()
    {
        if (!isChainActive && garbageObjects.Count > 0)
        {
            currentGarbageIndex = 0;
            StartCoroutine(CollectGarbage());
        }
    }

    // ��p toplama i�lemi
    private IEnumerator CollectGarbage()
    {
        if (currentGarbageIndex >= garbageObjects.Count) yield break;

        isChainActive = true;
        currentGarbage = garbageObjects[currentGarbageIndex];

        if (currentGarbage == null)
        {
            // E�er ��p art�k yoksa, bir sonrakine ge�
            currentGarbageIndex++;
            isChainActive = false;
            yield break;
        }

        // 1. Zinciri ��pe do�ru uzat
        Vector3 startPos = chainSpawnPoint.position;
        Vector3 targetPos = currentGarbage.transform.position;
        float distance = Vector3.Distance(startPos, targetPos);
        float currentDistance = 0f;

        while (currentDistance < distance)
        {
            currentDistance += chainSpeed * Time.deltaTime;
            currentDistance = Mathf.Min(currentDistance, distance);

            Vector3 currentPos = Vector3.Lerp(startPos, targetPos, currentDistance / distance);

            // Line Renderer g�ncelleme
            chainVisual.SetPosition(0, startPos);
            chainVisual.SetPosition(1, currentPos);

            yield return null;
        }

        // 2. ��p� yakala ve k�sa bir bekleme s�resi
        yield return new WaitForSeconds(collectionDelay);

        // 3. Zinciri ve ��p� geri �ek
        currentDistance = distance;

        while (currentDistance > 0)
        {
            currentDistance -= chainReturnSpeed * Time.deltaTime;
            currentDistance = Mathf.Max(0, currentDistance);

            Vector3 chainPos = Vector3.Lerp(startPos, targetPos, currentDistance / distance);

            // Line Renderer g�ncelleme
            chainVisual.SetPosition(0, startPos);
            chainVisual.SetPosition(1, chainPos);

            // ��p� de zincirle birlikte hareket ettir
            if (currentGarbage != null)
            {
                currentGarbage.transform.position = chainPos;
            }

            yield return null;
        }

        // 4. ��p� yok et veya i�le
        if (currentGarbage != null)
        {
            Destroy(currentGarbage);
        }

        // 5. Sonraki ��pe ge�
        currentGarbageIndex++;
        isChainActive = false;
    }
}