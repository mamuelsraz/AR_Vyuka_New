using pingak9;
using QRFoundation;
using TMPro;
using UnityEngine.SceneManagement;

public class QRPlacePage : Page
{
    public QRTrackerHandler tracker;
    public QRCodeTracker trackerBrain;
    public TextMeshProUGUI text;
    public Page viewPage;
    bool canScan = false;

    protected override void Awake()
    {
        base.Awake();
        onPreShow.AddListener(Initialize);
    }

    private void Start()
    {
        tracker.OnCodeAdded.AddListener(OnCodeDetected);
    }

    void Initialize()
    {
        SetScanning(false);
        SetScanning(true);
        text.text = "Stahuje se seznam objektů, vyčkejte";
        canScan = AssetStreamingManager.instance.ArObjectList.Length > 0;
        if (canScan)
        {
            text.text = "Hledá se QR kód";
        }
        else AssetStreamingManager.instance.LoadList(AssetStreamingManager.path, "list").Complete += (StreamingHandleResponse response) =>
        {
            if (response.status != StreamingStatus.Failed)
            {
                text.text = "Hledá se QR kód";
                canScan = true;
            }

            else
            {
                NativeDialog.OpenDialog("Nepovedlo se stáhnout seznam!", "Zkontrolujte prosím, zda máte stálé připojení k internetu. Aplikace se po stisknutí [ok] restartuje.", "Ok",
                () =>
                {
                    canScan = false;
                    SceneManager.LoadScene(0);
                });
            }
        };
    }

    void OnCodeDetected(QRCodeMarker marker)
    {
        if (!canScan) return;

        ArObject arObject = null;
        foreach (var item in AssetStreamingManager.instance.ArObjectList)
        {
            if (item.bundle == marker.content || item.nickName == marker.content) {
                arObject = item;
                break;
            }
        }

        if (arObject == null) {
            text.text = "QR kód nesedí s žádným objektem";
            return;
        }

        text.text = $"Stahuje se model s názvem: {arObject.nickName}";

        var handle = AssetStreamingManager.instance.LoadArObj(arObject, AssetStreamingManager.path);
        if (handle == null)
        {
            SelectedArObjectManager.instance.SelectNew(arObject);
            ArObjectDownloaded();
        }
        else handle.Complete += (StreamingHandleResponse response) =>
        {
            handle = null;
            if (response.status != StreamingStatus.Failed)
            {
                SelectedArObjectManager.instance.SelectNew(arObject);
                ArObjectDownloaded();
            }
            else
                NativeDialog.OpenDialog("Nepovedlo se stáhnout model!", "Zkontrolujte prosím, zda máte stálé připojení k internetu. Aplikace se po stisknutí [ok] restartuje.", "Ok",
                () =>
                {
                    SceneManager.LoadScene(0);
                });
        };
    }

    void ArObjectDownloaded() {
        QRCodeMarker marker = tracker.cachedCodes[SelectedArObjectManager.instance.selectedArObject.bundle];
        SelectedArObjectManager.instance.SpawnCurrent(marker.transform);

        GoTo(viewPage);
    }

    public void SetScanning(bool on)
    {
        if (on)
        {
            trackerBrain.enabled = true;
            tracker.enabled = true;
            trackerBrain.Reset();
        }
        else
        {
            trackerBrain.Unregister();
            trackerBrain.enabled = false;
            tracker.enabled = false;
        }
    }
}
