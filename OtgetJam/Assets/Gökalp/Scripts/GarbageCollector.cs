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
    public List<GameObject> garbageObjects = new List<GameObject>(); // Editor üzerinden çöpleri buraya sürükleyebilirsiniz

    private bool isChainActive = false;
    private GameObject currentGarbage;

    // Line Renderer bileþeni zinciri görselleþtirmek için
    private LineRenderer chainVisual;

    private int currentGarbageIndex = 0;

    void Start()
    {
        // Line Renderer component'ini al veya oluþtur
        chainVisual = GetComponent<LineRenderer>();
        if (!chainVisual)
        {
            chainVisual = gameObject.AddComponent<LineRenderer>();
        }

        // Line Renderer baþlangýç ayarlarý
        SetupLineRenderer();

        // Zincirin baþlangýç pozisyonunu ayarla
        chainVisual.positionCount = 2;
        chainVisual.SetPosition(0, chainSpawnPoint.position);
        chainVisual.SetPosition(1, chainSpawnPoint.position);
    }

    void SetupLineRenderer()
    {
        // Temel ayarlar
        chainVisual.startWidth = 0.1f;  // Zincirin baþlangýç geniþliði
        chainVisual.endWidth = 0.1f;    // Zincirin bitiþ geniþliði

        // Renk ayarlarý
        chainVisual.startColor = Color.gray;  // Zincirin baþlangýç rengi
        chainVisual.endColor = Color.gray;    // Zincirin bitiþ rengi
    }

    void Update()
    {
        // Eðer zincir aktif deðilse ve toplanacak çöp varsa, bir sonraki çöpü toplamaya baþla
        if (!isChainActive && currentGarbageIndex < garbageObjects.Count)
        {
            StartCoroutine(CollectGarbage());
        }
    }

    // Dýþarýdan çöpleri ayarlamak için (runtime sýrasýnda)
    public void SetGarbageList(List<GameObject> garbages)
    {
        garbageObjects = new List<GameObject>(garbages);
        currentGarbageIndex = 0;

        // Önceki toplama iþlemi varsa durdur
        StopAllCoroutines();
        isChainActive = false;

        // Zinciri baþlangýç pozisyonuna getir
        chainVisual.SetPosition(0, chainSpawnPoint.position);
        chainVisual.SetPosition(1, chainSpawnPoint.position);
    }

    // Toplama iþlemini manuel olarak baþlatmak için
    public void StartCollection()
    {
        if (!isChainActive && garbageObjects.Count > 0)
        {
            currentGarbageIndex = 0;
            StartCoroutine(CollectGarbage());
        }
    }

    // Çöp toplama iþlemi
    private IEnumerator CollectGarbage()
    {
        if (currentGarbageIndex >= garbageObjects.Count) yield break;

        isChainActive = true;
        currentGarbage = garbageObjects[currentGarbageIndex];

        if (currentGarbage == null)
        {
            // Eðer çöp artýk yoksa, bir sonrakine geç
            currentGarbageIndex++;
            isChainActive = false;
            yield break;
        }

        // 1. Zinciri çöpe doðru uzat
        Vector3 startPos = chainSpawnPoint.position;
        Vector3 targetPos = currentGarbage.transform.position;
        float distance = Vector3.Distance(startPos, targetPos);
        float currentDistance = 0f;

        while (currentDistance < distance)
        {
            currentDistance += chainSpeed * Time.deltaTime;
            currentDistance = Mathf.Min(currentDistance, distance);

            Vector3 currentPos = Vector3.Lerp(startPos, targetPos, currentDistance / distance);

            // Line Renderer güncelleme
            chainVisual.SetPosition(0, startPos);
            chainVisual.SetPosition(1, currentPos);

            yield return null;
        }

        // 2. Çöpü yakala ve kýsa bir bekleme süresi
        yield return new WaitForSeconds(collectionDelay);

        // 3. Zinciri ve çöpü geri çek
        currentDistance = distance;

        while (currentDistance > 0)
        {
            currentDistance -= chainReturnSpeed * Time.deltaTime;
            currentDistance = Mathf.Max(0, currentDistance);

            Vector3 chainPos = Vector3.Lerp(startPos, targetPos, currentDistance / distance);

            // Line Renderer güncelleme
            chainVisual.SetPosition(0, startPos);
            chainVisual.SetPosition(1, chainPos);

            // Çöpü de zincirle birlikte hareket ettir
            if (currentGarbage != null)
            {
                currentGarbage.transform.position = chainPos;
            }

            yield return null;
        }

        // 4. Çöpü yok et veya iþle
        if (currentGarbage != null)
        {
            Destroy(currentGarbage);
        }

        // 5. Sonraki çöpe geç
        currentGarbageIndex++;
        isChainActive = false;
    }
}