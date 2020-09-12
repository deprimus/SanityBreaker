using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deprimus : MonoBehaviour
{
    private new SpriteRenderer renderer;
    private Color color;
    public bool consumed;

    public new Transform transform;

    public static int crizatusCount = 0;
    public static int confusciusCount = 0;
    public static int curiosusCount = 0;
    public static int ordinariusCount = 0;
    public static int disgustusCount = 0;
    public static int obsedatusCount = 0;
    public static int hlizatusCount = 0;
    public static int nervosusCount = 0;

    public enum Type
    {
        DEPRIMUS,
        AGILLIUS,
        CRIZATUS,
        DILIUS,
        CONFUSCIUS,
        INGINIUS,
        FOOLUS,
        ORDINARIUS,
        CURIOSUS,
        DISGUSTUS,
        CHANSUS,
        GHINIONUS,
        FERICITUS,
        OBSEDATUS,
        HLIZATUS,
        NERVOSUS
    }

    public Type type;

    public void Construct(Type type) => this.type = type;

    void Start()
    {
        transform = GetComponent<Transform>();
        renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = Resources.Load<Sprite>("Sprites/Deprimus/" + type.ToString()[0] + type.ToString().Substring(1).ToLower());

        consumed = false;

        color = renderer.material.color;
        color.a = 0f;
        renderer.material.color = color;

        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        for(float i = 0f; i <= 1f; i += 0.05f)
        {
            if(renderer == null)
                yield break;

            color.a = i;
            renderer.material.color = color;

            yield return new WaitForSeconds(0.0078125f);
        }
    }

    public IEnumerator FadeOut()
    {
        SoundManager.Play((SoundManager.Clip) Enum.Parse(typeof(SoundManager.Clip), type.ToString(), false));

        for (float i = 1f; i >= 0f; i -= 0.05f)
        {
            if(renderer == null)
                yield break;

            color.a = i;
            renderer.material.color = color;

            yield return new WaitForSeconds(0.0078125f);
        }

        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!consumed && other.gameObject.tag.Equals("Ball"))
        {
            consumed = true;

            switch(type)
            {
                case Type.DEPRIMUS:
                    DeprimusManager.Consume(this, 0f, () => {
                        for(byte i = 0; i < 2; ++i)
                            DeprimusManager.Spawn();
                    }, null);
                    break;
                case Type.AGILLIUS:
                    DeprimusManager.Consume(this, 5f, () => {
                        GameManager.ball.speed *= 2.0f;
                        GameManager.ball.rigidBody.velocity *= 2.0f;
                    }, () => {
                        GameManager.ball.speed /= 2.0f;
                        GameManager.ball.rigidBody.velocity /= 2.0f;
                    });
                    break;
                case Type.CRIZATUS:
                    DeprimusManager.Consume(this, 5f, () => {
                        ++crizatusCount;

                        if(crizatusCount == 1)
                            GameManager.crizatusCanvas.SetActive(true);
                    }, () => {
                        --crizatusCount;

                        if (crizatusCount == 0)
                        {
                            GameManager.crizatusCanvas.SetActive(false);
                            MainCamera.transform.localPosition = MainCamera.basePos;
                        }
                    });
                    break;
                case Type.DILIUS:
                    DeprimusManager.Consume(this, 5f, () => {
                        DeprimusManager.actualSpawnProb *= 3.0f;
                    }, () => {
                        DeprimusManager.actualSpawnProb /= 3.0f;
                    });
                    break;
                case Type.CONFUSCIUS:
                    DeprimusManager.Consume(this, 5f, () => {
                        ++confusciusCount;

                        if(confusciusCount == 1)
                            GameManager.player.isConfused = true;
                    }, () => {
                        --confusciusCount;

                        if(confusciusCount == 0)
                            GameManager.player.isConfused = false;
                    });
                    break;
                case Type.INGINIUS:
                    DeprimusManager.Consume(this, 0f, () => {
                        for(byte i = 0; i < 2; ++i)
                            DeprimusManager.SpawnOfType(Type.CHANSUS);
                    }, null);
                    break;
                case Type.FOOLUS:
                    DeprimusManager.Consume(this, 0f, () => {
                        for(byte i = 0; i < 2; ++i)
                            DeprimusManager.SpawnOfType(Type.GHINIONUS);
                    }, null);
                    break;
                case Type.ORDINARIUS:
                    DeprimusManager.Consume(this, 5f, () => {
                        ++ordinariusCount;
                    }, () => {
                        --ordinariusCount;

                        if (ordinariusCount == 0)
                        {
                            GameManager.ball.transform.eulerAngles = Vector3.zero;
                            GameManager.ball.rigidBody.velocity = GameManager.ball.baseVelocity * GameManager.ball.speed;
                        }
                    });
                    break;
                case Type.CURIOSUS:
                    DeprimusManager.Consume(this, 5f, () => {
                        ++curiosusCount;
                    }, () => {
                        --curiosusCount;

                        if (curiosusCount == 0)
                        {
                            GameManager.ball.transform.eulerAngles = Vector3.zero;
                            GameManager.ball.rigidBody.velocity = GameManager.ball.baseVelocity * GameManager.ball.speed;
                        }
                    });
                    break;
                case Type.DISGUSTUS:
                    DeprimusManager.Consume(this, 5f, () => {
                        ++disgustusCount;

                        if (disgustusCount == 1)
                        {
                            GameManager.player.isDisgusted = true;
                            GameManager.player.renderer.material.color = new Color(128 / 255f, 42 / 255f, 77 / 255f);
                        }
                    }, () => {
                        --disgustusCount;

                        if (disgustusCount == 0)
                        {
                            GameManager.player.isDisgusted = false;
                            GameManager.player.renderer.material.color = new Color(1.0f, 1.0f, 1.0f);
                        }
                    });
                    break;
                case Type.CHANSUS:
                    DeprimusManager.Consume(this, 0f, () => {
                        int count = Mathf.Min(BrickManager.bricks.Count, UnityEngine.Random.Range(1, 4));

                        while((count--) > 0)
                            BrickManager.DestroyBrick(BrickManager.bricks[UnityEngine.Random.Range(0, BrickManager.bricks.Count)]);
                    } , null);
                    break;
                case Type.GHINIONUS:
                    DeprimusManager.Consume(this, 0f, () => {
                        int count = Mathf.Min(BrickManager.freePositions.Count, UnityEngine.Random.Range(1, 4));

                        while((count--) > 0)
                            BrickManager.RecreateBrick(UnityEngine.Random.Range(0, BrickManager.freePositions.Count));
                    }, null);
                    break;
                case Type.FERICITUS:
                    DeprimusManager.Consume(this, 0f, () => DeprimusManager.Reset(), null);
                    break;
                case Type.HLIZATUS:
                    DeprimusManager.Consume(this, 5f, () => {
                        ++hlizatusCount;
                    }, () => {
                        --hlizatusCount;

                        if(hlizatusCount == 0)
                            MainCamera.transform.localPosition = MainCamera.basePos;
                    });
                    break;
                case Type.OBSEDATUS:
                    DeprimusManager.Consume(this, 5f, () => {
                        ++obsedatusCount;

                        if(obsedatusCount == 1)
                            GameManager.player.isObsessed = true;
                    }, () => {
                        --obsedatusCount;

                        if(obsedatusCount == 0)
                            GameManager.player.isObsessed = false;
                    });
                    break;
                case Type.NERVOSUS:
                    DeprimusManager.Consume(this, 5f, () => {
                        ++nervosusCount;

                        if (nervosusCount == 1)
                            GameManager.nervosusCanvas.SetActive(true);
                    }, () => {
                        --nervosusCount;

                        if (nervosusCount == 0)
                        {
                            GameManager.nervosusCanvas.SetActive(false);
                            MainCamera.transform.localPosition = MainCamera.basePos;
                        }
                    });
                    break;
            }
        }
    }
}
