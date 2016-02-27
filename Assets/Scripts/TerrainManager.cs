using UnityEngine;
using System.Collections;

public class TerrainManager : MonoBehaviour {

	public Sprite[] floorSprites;

	public int floorWidth = 25;
	public int floorHeight = 25;

	public int key = 1;

	public Transform player;

	public int maxDistanceFromCenter = 7;

	private SpriteRenderer[,] _renderers;


	Sprite SelectRandomSprite(int x, int y)
	{
		return floorSprites [RandomHelper.Range(x, y, key, floorSprites.Length)];
	}


	void RedrawMap()
	{
		transform.position = new Vector3 ((int)player.position.x, (int)player.position.y, player.position.z);	// Сдвигаем позитцию карты в позицию игрока

		for (int x = 0; x < floorWidth; x++)
			for (int y = 0; y < floorHeight; y++) {

				SpriteRenderer spriteRenderer = _renderers[x, y];								// Берем из массива спрайтов нужный спрайт
				spriteRenderer.sprite = SelectRandomSprite((int)transform.position.x + x, (int)transform.position.y + y);		// Указываем сам спрайт (берем случайный с учетом сдвига карты) )

			}
	}


	// Use this for initialization
	void Start () {

		var tileOffset = new Vector3 (0 - floorWidth / 2, 0 - floorHeight / 2, 0f);

		_renderers = new SpriteRenderer[floorWidth, floorHeight];

		for (int x = 0; x < floorWidth; x++)
			for (int y = 0; y < floorHeight; y++) {

				GameObject tile = new GameObject ();											// Создаем объект

				tile.transform.position = new Vector3 (x, y, 0f) + tileOffset;					// Присваеваем ему координаты

				_renderers[x, y] = tile.AddComponent<SpriteRenderer>();							// Добавляем в массив рендер спрайта

				tile.name = "Floor " + tile.transform.position;									// Имя спрайта (название + позиция)
				tile.transform.parent = transform;												// Прописываем родителя (transform???)
			}

		RedrawMap ();
	
	}
	
	// Update is called once per frame
	void Update () {

		if (maxDistanceFromCenter < Vector3.Distance(player.position, transform.position)) {
			RedrawMap ();
		}
	
	}
}
