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

        var handle = AddressablesStreamingManager.Instance.LoadAssetCatalog();
        if (handle == null)
        {
            text.text = "Hledá se QR kód";
            canScan = true;
        }
        else
        {
            handle.OnComplete += (response) =>
            {
                text.text = "Hledá se QR kód";
                canScan = true;
            };
            handle.OnFail += () =>
            {
                NativeDialog.OpenDialog("Nepovedlo se stáhnout seznam!", "Zkontrolujte prosím, zda máte stálé připojení k internetu. Aplikace se po stisknutí [ok] restartuje.", "Ok",
                () =>
                {
                    canScan = false;
                    SceneManager.LoadScene(0);
                });
            };
        }
    }

    void OnCodeDetected(QRCodeMarker marker)
    {
        if (!canScan) return;

        LanguageARAsset ARAsset = null;
        foreach (var item in AddressablesStreamingManager.Instance.catalog.assets)
        {
            if (item.nickName == marker.content)
            {
                ARAsset = item;
                break;
            }
        }

        if (ARAsset == null)
        {
            text.text = "QR kód nesedí s žádným objektem";
            return;
        }

        text.text = $"Stahuje se model s názvem: {ARAsset.nickName}";

        var handle = AddressablesStreamingManager.Instance.LoadArObj(ARAsset);
        if (handle == null)
        {
            SelectedArObjectManager.instance.SelectNew(ARAsset);
            ArObjectDownloaded();
        }
        else
        {
            handle.OnFail += () =>
            {
                NativeDialog.OpenDialog("Nepovedlo se stáhnout model!", "Zkontrolujte prosím, zda máte stálé připojení k internetu. Aplikace se po stisknutí [ok] restartuje.", "Ok",
                 () =>
                 {
                     SceneManager.LoadScene(0);
                 });
            };
            handle.OnComplete += (response) =>
            {
                SelectedArObjectManager.instance.SelectNew(ARAsset);
                ArObjectDownloaded();
            };
        }
    }

    void ArObjectDownloaded()
    {
        QRCodeMarker marker = tracker.cachedCodes[SelectedArObjectManager.instance.selectedARAsset.nickName];
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
