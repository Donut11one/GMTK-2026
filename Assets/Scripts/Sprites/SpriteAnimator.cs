using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimator : MonoBehaviour
{
    [SerializeField] private float fps = 8f;

    private SpriteRenderer _renderer;
    private Sprite[] _frames;
    private int _index;
    private float _timer;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    public void Play(Sprite[] frames)
    {
        if (frames == _frames)
        {
            return;
        }

        _frames = frames;
        Restart();
    }

    public void Restart()
    {
        _index = 0;
        _timer = 0f;

        if (_frames != null &&
            _frames.Length > 0
        ){
            _renderer.sprite = _frames[0];
        }
    }

    public void SetFlip(bool flipX)
    {
        _renderer.flipX = flipX;
    }

    private void Update()
    {
        if (_frames == null ||
            _frames.Length == 0
        ){
            return;
        }

        _timer += Time.deltaTime;
        float duration = 1f / fps;

        if (_timer >= duration)
        {
            _timer -= duration;
            _index = (_index + 1) % _frames.Length;
            _renderer.sprite = _frames[_index];
        }
    }
}
