using UnityEngine;
using System.Collections;

public class RandomLevelGenerator : MonoBehaviour {

	public int maxSteps = 1000;

	public int levelWidth = 100;
	public int levelHeight = 100;

	public GameObject enemy;

	public Sprite floorVOID;
	public Sprite floor01;
	public Sprite floorWALL;

	public Sprite floorWALL_top_left;
	public Sprite floorWALL_top;
	public Sprite floorWALL_top_right;

	public Sprite floorWALL_left;
	public Sprite floorWALL_right;

	public Sprite floorWALL_bottom_left;
	public Sprite floorWALL_bottom;
	public Sprite floorWALL_bottom_right;

	public Sprite floorWALL_corner_top_left;
	public Sprite floorWALL_corner_top_right;
	public Sprite floorWALL_corner_bottom_left;
	public Sprite floorWALL_corner_bottom_right;

	private int[,] grid;

	private const int floor_VOID = 0;
	private const int floor_01 = 100;
	private const int floor_WALL = 1000;

	private const int floor_WALL_top_left = 1;
	private const int floor_WALL_top = 2;
	private const int floor_WALL_top_right = 3;

	private const int floor_WALL_left = 4;
	//private const int floor_WALL_left_sidetop = 44;

	private const int floor_WALL_right = 6;
	//private const int floor_WALL_right_sidetop = 66;

	private const int floor_WALL_bottom_left = 7;
	private const int floor_WALL_bottom = 8;
	private const int floor_WALL_bottom_right = 9;

	private const int floor_WALL_corner_top_left = 10;
	private const int floor_WALL_corner_top_right = 11;
	private const int floor_WALL_corner_bottom_left = 12;
	private const int floor_WALL_corner_bottom_right = 13;

	private GameObject[,] gridGObjects;
	private SpriteRenderer[,] gridSpriteRenderers;

	private int keySpaceUpPressed = 1;

	private bool top_left;
	private bool top;
	private bool top_right;

	private bool left;
	private bool right;

	private bool bottom_left;
	private bool bottom;
	private bool bottom_right;

	// Размеры блоков 2D коллайдеров
	private Vector2 wcSize_full;
	private Vector2 wcSize_medium_vert;
	private Vector2 wcSize_medium_horiz;
	private Vector2 wcSize_small;


	// Генерируем уровень
	void CreateRandomLevel() {

		// Заполнем уровень пустотой
		for (int y = 0; y < levelHeight; y++)
			for (int x = 0; x < levelWidth; x++)
				grid[x, y] = floor_VOID;														// Указываем номер спрайта

		// Заполняем уровень, начиная с центра
		int cx = levelWidth / 2;																// Помещаем контроллер в центр карты
		int cy = levelHeight / 2;

		for (int i = 0; i < maxSteps; i++) {
			grid [cx, cy] = floor_01;															// Устанавливаем пол с текущими координатами контроллера
			grid [cx + 1, cy] = floor_01;
			grid [cx, cy + 1] = floor_01;
			grid [cx + 1, cy + 1] = floor_01;

			cx += Random.Range (-1, 2);
			cy += Random.Range (-1, 2);

			// Проверяем, не вышли ли мы за пределы карты
			if (cx > levelWidth - 4) cx = levelWidth - 4;
			else if (cx < 2) cx = 2;

			if (cy > levelHeight - 4) cy = levelHeight - 4;
			else if (cy < 2) cy = 2;

		}

		bool probel_0;
		bool probel_x1;
		bool probel_x2;
		bool probel_x3;

		bool probel_y1;
		bool probel_y2;
		bool probel_y3;

		// Проверяем на заполненную клетку между двумя соседними пустыми. Если нашли, то заменяем третью и четвертую на заполненные
		for (int y = 1; y < levelHeight - 4; y++)
			for (int x = 1; x < levelWidth - 4; x++) {

				probel_0 = grid [x, y] == floor_VOID;

				probel_x1 = grid [x + 1, y] == floor_VOID;
				probel_x2 = grid [x + 2, y] == floor_VOID;
				probel_x3 = grid [x + 3, y] == floor_VOID;

				probel_y1 = grid [x, y + 1] == floor_VOID;
				probel_y2 = grid [x, y + 2] == floor_VOID;
				probel_y3 = grid [x, y + 3] == floor_VOID;

				//Проверяем по оси X
				if (probel_0 && !probel_x1 && probel_x2) {
					grid [x + 2, y] = floor_01;
					grid [x + 3, y] = floor_01;
				}

				//Проверяем по оси Y
				if (probel_0 && !probel_y1 && probel_y2) {
					grid [x, y + 2] = floor_01;
					grid [x, y + 3] = floor_01;
				}
			}


		// Проверяем на 2 заполненные клетки подряд между двумя пустыми. Если нашли, то заменяем на 3 заполненные подряд
		for (int y = 1; y < levelHeight - 4; y++)
			for (int x = 1; x < levelWidth - 4; x++) {

				probel_0 = grid [x, y] != floor_VOID;

				probel_x1 = grid [x + 1, y] != floor_VOID;
				probel_x2 = grid [x + 2, y] != floor_VOID;
				probel_x3 = grid [x + 3, y] != floor_VOID;

				probel_y1 = grid [x, y + 1] != floor_VOID;
				probel_y2 = grid [x, y + 2] != floor_VOID;
				probel_y3 = grid [x, y + 3] != floor_VOID;

				//Проверяем по оси X
				if (!probel_0 && probel_x1 && probel_x2 && !probel_x3)
					grid [x + 3, y] = floor_01;

				//Проверяем по оси Y
				if (!probel_0 && probel_y1 && probel_y2 && !probel_y3)
					grid [x, y + 3] = floor_01;
				
			}

		// Проверяем на одиночные клеточки. С трех сторон окруженные пустотой. если нашли, то заменяем на пустоту
		for (int y = 2; y < levelHeight - 2; y++)
			for (int x = 2; x < levelWidth - 2; x++) {

				probel_0 = grid [x, y] != floor_VOID;

				probel_x1 = grid [x + 1, y] != floor_VOID;
				probel_x2 = grid [x - 1, y] != floor_VOID;

				probel_y1 = grid [x, y + 1] != floor_VOID;
				probel_y2 = grid [x, y - 1] != floor_VOID;

				if (probel_0) {
					
					if (!probel_y1 && !probel_x1 && !probel_y2)
						grid [x, y] = floor_VOID;

					if (!probel_x1 && !probel_y2 && !probel_x2)
						grid [x, y] = floor_VOID;

					if (!probel_y2 && !probel_x2 && !probel_y1)
						grid [x, y] = floor_VOID;

					if (!probel_x2 && !probel_y1 && !probel_x1)
						grid [x, y] = floor_VOID;

				}

			}

		// Расставляем врагов
		for (int y = 0; y < levelHeight; y++)
			for (int x = 0; x < levelWidth; x++) {

				if (grid [x, y] == floor_01 && Random.Range(0, 100f) > 98f) {

					Instantiate (enemy, new Vector3 (x, y, 0f), Quaternion.identity);

				}

			}
				

		// Расставляем стенки
		for (int y = 1; y < levelHeight - 1; y++)
			for (int x = 1; x < levelWidth - 1; x++) {
					
				/*if (grid [x, y] != floor_VOID)
					grid [x, y] = floor_WALL;*/
		
			
				if (grid [x, y] == floor_VOID) {													// Если текущая клеточка пол, то смотрим клетки вокруг нее

					// Проверяем соседние клеточки, что они не являются полом
					top_left = grid [x - 1, y + 1] != floor_VOID;
					top = grid [x, y + 1] != floor_VOID;
					top_right = grid [x + 1, y + 1] != floor_VOID;

					left = grid [x - 1, y] != floor_VOID;
					right = grid [x + 1, y] != floor_VOID;

					bottom_left = grid [x - 1, y - 1] != floor_VOID;
					bottom = grid [x, y - 1] != floor_VOID;
					bottom_right = grid [x + 1, y - 1] != floor_VOID;

					// Назначаем стенки





					// LEFT
					if (left) {

						if (top) {

							if (top_left)
								grid [x - 1, y + 1] = floor_WALL_corner_top_left;

							if (top_left && bottom_left)
								grid [x - 1, y] = floor_WALL_right;

							if (top_right)
								grid [x, y + 1] = floor_WALL_top;

							else
								grid [x, y + 1] = floor_WALL_bottom_right;

						}

						else if (top_left && !bottom_left)
							grid [x - 1, y] = floor_WALL_bottom_right;

						else
							grid [x - 1, y] = floor_WALL_right;

						if (bottom && bottom_left)
							grid [x - 1, y - 1] = floor_WALL_corner_bottom_left;
					}





					// RIGHT
					if (right) {

						if (top) {

							if (top_right)
								grid [x + 1, y + 1] = floor_WALL_corner_top_right;

							if (top_right && bottom_right)
								grid [x + 1, y] = floor_WALL_left;

							if (top_left)
								grid [x, y + 1] = floor_WALL_top;

							else
								grid [x, y + 1] = floor_WALL_bottom_left;

						}

						else if (top_right && !bottom_right)
							grid [x + 1, y] = floor_WALL_bottom_left;

						else
							grid [x + 1, y] = floor_WALL_left;

						if (bottom && bottom_right)
							grid [x + 1, y - 1] = floor_WALL_corner_bottom_right;
					}




					// TOP
					if (top) {
						
						if (top_left && top_right)
							grid [x, y + 1] = floor_WALL_bottom;

						else if (!top_left && top_right)
							grid [x, y + 1] = floor_WALL_bottom_left;

						else if (top_left && !top_right)
							grid [x, y + 1] = floor_WALL_bottom_right;

					}




					// BOTTOM
					if (bottom) {

						if (bottom_left && bottom_right)
							grid [x, y - 1] = floor_WALL_top;

						else if (!bottom_left && bottom_right)
							grid [x, y - 1] = floor_WALL_top_left;

						else if (bottom_left && !bottom_right)
							grid [x, y - 1] = floor_WALL_top_right;

					}


				}
			}

		//Меняем спрайты по сгенерированному уровню
		for (int y = 0; y < levelHeight; y++)
			for (int x = 0; x < levelWidth; x++) {

				SpriteRenderer spriteRenderer = gridSpriteRenderers[x, y];

				//GameObject curObject = GameObject.Find ("Floor_" + x + "_" + y);
				GameObject curObject = gridGObjects[x, y];
				BoxCollider2D[] curAllColliders;

				// Если нашли объект, то продолжаем дальше
				if (curObject) {

					// Пустота --------------------------------------------------------------------------------------------------------
					if (grid [x, y] == floor_VOID) {
						
						spriteRenderer.sprite = floorVOID;
						spriteRenderer.sortingLayerName = "default";

						curAllColliders = curObject.GetComponents<BoxCollider2D> ();								// Пытаемся получить его BoxCollider2D

						if (curAllColliders.Length > 0)
							foreach (BoxCollider2D tileBC2D in curAllColliders) {

								Destroy (tileBC2D);																			// Если нашли BoxCollider2D, то удаляем
								//Debug.Log ("Удалили коллайдер");

							}


						// Стена целиком --------------------------------------------------------------------------------------------------------
					} else if (grid [x, y] == floor_WALL) {

						spriteRenderer.sprite = floorWALL;
						spriteRenderer.sortingLayerName = "default";

						curAllColliders = curObject.GetComponents<BoxCollider2D> ();								// Пытаемся получить его BoxCollider2D

						if (curAllColliders.Length > 0)
							foreach (BoxCollider2D tileBC2D in curAllColliders) {

								Destroy (tileBC2D);																			// Если нашли BoxCollider2D, то удаляем
								//Debug.Log ("Удалили коллайдер");

							}

	
						// Стена top_left --------------------------------------------------------------------------------------------------------
					} else if (grid [x, y] == floor_WALL_top_left) {
						
						spriteRenderer.sprite = floorWALL_top_left;
						spriteRenderer.sortingLayerName = "default";

						curAllColliders = curObject.GetComponents<BoxCollider2D> ();									// Пытаемся получить список его BoxCollider2D

						if (curAllColliders.Length == 0) {

							BoxCollider2D tileBC2D = curObject.AddComponent<BoxCollider2D> ();

							tileBC2D.size = wcSize_medium_horiz;
							tileBC2D.offset = new Vector2 (0.5f, 0.81f);

							tileBC2D = curObject.AddComponent<BoxCollider2D> ();

							tileBC2D.size = wcSize_medium_vert;
							tileBC2D.offset = new Vector2 (0.19f, 0.5f);

						} else if (curAllColliders.Length == 1) {

							curAllColliders [0].size = wcSize_medium_horiz;
							curAllColliders [0].offset = new Vector2 (0.5f, 0.81f);
							curAllColliders [0].enabled = true;

							BoxCollider2D tileBC2D = curObject.AddComponent<BoxCollider2D> ();

							tileBC2D.size = wcSize_medium_vert;
							tileBC2D.offset = new Vector2 (0.19f, 0.5f);

						} else if (curAllColliders.Length == 2) {

							curAllColliders [0].size = wcSize_medium_horiz;
							curAllColliders [0].offset = new Vector2 (0.5f, 0.81f);
							curAllColliders [0].enabled = true;

							curAllColliders [1].size = wcSize_medium_vert;
							curAllColliders [1].offset = new Vector2 (0.19f, 0.5f);
							curAllColliders [1].enabled = true;

						} else {
							Debug.Log ("Ошибка. Не может быть больше 2 коллайдеров");
							return;
						}


						// Стена top --------------------------------------------------------------------------------------------------------
					} else if (grid [x, y] == floor_WALL_top) {

						spriteRenderer.sprite = floorWALL_top;
						spriteRenderer.sortingLayerName = "default";

						curAllColliders = curObject.GetComponents<BoxCollider2D> ();									// Пытаемся получить список его BoxCollider2D

						if (curAllColliders.Length == 0) {

							//Debug.Log ("Создали коллайдер");
							BoxCollider2D tileBC2D = curObject.AddComponent<BoxCollider2D> ();

							tileBC2D.size = wcSize_medium_horiz;
							tileBC2D.offset = new Vector2 (0.5f, 0.81f);

						} else if (curAllColliders.Length == 1) {

							//Debug.Log ("Нашли 1 коллайдер");

							curAllColliders [0].size = wcSize_medium_horiz;
							curAllColliders [0].offset = new Vector2 (0.5f, 0.81f);
							curAllColliders [0].enabled = true;

						} else if (curAllColliders.Length == 2) {

							//Debug.Log ("Нашли 2 коллайдера");

							curAllColliders [0].size = wcSize_medium_horiz;
							curAllColliders [0].offset = new Vector2 (0.5f, 0.81f);
							curAllColliders [0].enabled = true;

							curAllColliders [1].enabled = false;

						} else {
							Debug.Log ("Ошибка. Не может быть больше 2 коллайдеров");
							return;
						}


						// Стена top_right --------------------------------------------------------------------------------------------------------
					} else if (grid [x, y] == floor_WALL_top_right) {

						spriteRenderer.sprite = floorWALL_top_right;
						spriteRenderer.sortingLayerName = "default";

						curAllColliders = curObject.GetComponents<BoxCollider2D> ();									// Пытаемся получить список его BoxCollider2D

						if (curAllColliders.Length == 0) {
							
							BoxCollider2D tileBC2D = curObject.AddComponent<BoxCollider2D> ();

							tileBC2D.size = wcSize_medium_horiz;
							tileBC2D.offset = new Vector2 (0.5f, 0.81f);

							tileBC2D = curObject.AddComponent<BoxCollider2D> ();

							tileBC2D.size = wcSize_medium_vert;
							tileBC2D.offset = new Vector2 (0.81f, 0.5f);

						} else if (curAllColliders.Length == 1) {

							curAllColliders [0].size = wcSize_medium_horiz;
							curAllColliders [0].offset = new Vector2 (0.5f, 0.81f);
							curAllColliders [0].enabled = true;

							BoxCollider2D tileBC2D = curObject.AddComponent<BoxCollider2D> ();

							tileBC2D.size = wcSize_medium_vert;
							tileBC2D.offset = new Vector2 (0.5f, 0.81f);

						} else if (curAllColliders.Length == 2) {

							curAllColliders [0].size = wcSize_medium_horiz;
							curAllColliders [0].offset = new Vector2 (0.5f, 0.81f);
							curAllColliders [0].enabled = true;

							curAllColliders [1].size = wcSize_medium_vert;
							curAllColliders [1].offset = new Vector2 (0.81f, 0.5f);
							curAllColliders [1].enabled = true;

						} else {
							Debug.Log ("Ошибка. Не может быть больше 2 коллайдеров");
							return;
						}


						// Стена left --------------------------------------------------------------------------------------------------------
					} else if (grid [x, y] == floor_WALL_left) {

						spriteRenderer.sprite = floorWALL_left;
						spriteRenderer.sortingLayerName = "default";

						curAllColliders = curObject.GetComponents<BoxCollider2D> ();									// Пытаемся получить список его BoxCollider2D

						if (curAllColliders.Length == 0) {

							//Debug.Log ("Создали коллайдер");
							BoxCollider2D tileBC2D = curObject.AddComponent<BoxCollider2D> ();

							tileBC2D.size = wcSize_medium_vert;
							tileBC2D.offset = new Vector2 (0.19f, 0.5f);
								
						} else if (curAllColliders.Length == 1) {

							//Debug.Log ("Нашли 1 коллайдер");

							curAllColliders [0].size = wcSize_medium_vert;
							curAllColliders [0].offset = new Vector2 (0.19f, 0.5f);
							curAllColliders [0].enabled = true;

						} else if (curAllColliders.Length == 2) {

							//Debug.Log ("Нашли 2 коллайдера");

							curAllColliders [0].size = wcSize_medium_vert;
							curAllColliders [0].offset = new Vector2 (0.19f, 0.5f);
							curAllColliders [0].enabled = true;

							curAllColliders [1].enabled = false;

						} else {
							Debug.Log ("Ошибка. Не может быть больше 2 коллайдеров");
							return;
						}




					// Стена right --------------------------------------------------------------------------------------------------------
					} else if (grid [x, y] == floor_WALL_right) {

						spriteRenderer.sprite = floorWALL_right;
						spriteRenderer.sortingLayerName = "default";

						curAllColliders = curObject.GetComponents<BoxCollider2D> ();									// Пытаемся получить список его BoxCollider2D

						if (curAllColliders.Length == 0) {

							//Debug.Log ("Создали коллайдер");
							BoxCollider2D tileBC2D = curObject.AddComponent<BoxCollider2D> ();

							tileBC2D.size = wcSize_medium_vert;
							tileBC2D.offset = new Vector2 (0.81f, 0.5f);

						} else if (curAllColliders.Length == 1) {

							//Debug.Log ("Нашли 1 коллайдер");

							curAllColliders [0].size = wcSize_medium_vert;
							curAllColliders [0].offset = new Vector2 (0.81f, 0.5f);
							curAllColliders [0].enabled = true;

						} else if (curAllColliders.Length == 2) {

							//Debug.Log ("Нашли 2 коллайдера");

							curAllColliders [0].size = wcSize_medium_vert;
							curAllColliders [0].offset = new Vector2 (0.81f, 0.5f);
							curAllColliders [0].enabled = true;

							curAllColliders [1].enabled = false;

						} else {
							Debug.Log ("Ошибка. Не может быть больше 2 коллайдеров");
							return;
						}


					// Стена bottom_left --------------------------------------------------------------------------------------------------------
					} else if (grid [x, y] == floor_WALL_bottom_left) {

						spriteRenderer.sprite = floorWALL_bottom_left;
						spriteRenderer.sortingLayerName = "top_walls";

						curAllColliders = curObject.GetComponents<BoxCollider2D> ();									// Пытаемся получить список его BoxCollider2D

						if (curAllColliders.Length == 0) {

							//Debug.Log ("Создали коллайдер");
							BoxCollider2D tileBC2D = curObject.AddComponent<BoxCollider2D> ();

							tileBC2D.size = wcSize_medium_horiz;
							tileBC2D.offset = new Vector2 (0.5f, 0.19f);

							tileBC2D = curObject.AddComponent<BoxCollider2D> ();

							tileBC2D.size = wcSize_medium_vert;
							tileBC2D.offset = new Vector2 (0.19f, 0.5f);

						} else if (curAllColliders.Length == 1) {

							//Debug.Log ("Нашли 1 коллайдер");

							curAllColliders [0].size = wcSize_medium_horiz;
							curAllColliders [0].offset = new Vector2 (0.5f, 0.19f);
							curAllColliders [0].enabled = true;

							BoxCollider2D tileBC2D = curObject.AddComponent<BoxCollider2D> ();

							tileBC2D.size = wcSize_medium_vert;
							tileBC2D.offset = new Vector2 (0.19f, 0.5f);

						} else if (curAllColliders.Length == 2) {

							//Debug.Log ("Нашли 2 коллайдера");

							curAllColliders [0].size = wcSize_medium_horiz;
							curAllColliders [0].offset = new Vector2 (0.5f, 0.19f);
							curAllColliders [0].enabled = true;

							curAllColliders [1].size = wcSize_medium_vert;
							curAllColliders [1].offset = new Vector2 (0.19f, 0.5f);
							curAllColliders [1].enabled = true;

						} else {
							Debug.Log ("Ошибка. Не может быть больше 2 коллайдеров");
							return;
						}


					// Стена bottom --------------------------------------------------------------------------------------------------------
					} else if (grid [x, y] == floor_WALL_bottom) {

						spriteRenderer.sprite = floorWALL_bottom;
						spriteRenderer.sortingLayerName = "top_walls";

						curAllColliders = curObject.GetComponents<BoxCollider2D> ();									// Пытаемся получить список его BoxCollider2D

						if (curAllColliders.Length == 0) {

							//Debug.Log ("Создали коллайдер");
							BoxCollider2D tileBC2D = curObject.AddComponent<BoxCollider2D> ();

							tileBC2D.size = wcSize_medium_horiz;
							tileBC2D.offset = new Vector2 (0.5f, 0.19f);

						} else if (curAllColliders.Length == 1) {

							//Debug.Log ("Нашли 1 коллайдер");

							curAllColliders [0].size = wcSize_medium_horiz;
							curAllColliders [0].offset = new Vector2 (0.5f, 0.19f);
							curAllColliders [0].enabled = true;

						} else if (curAllColliders.Length == 2) {

							//Debug.Log ("Нашли 2 коллайдера");

							curAllColliders [0].size = wcSize_medium_horiz;
							curAllColliders [0].offset = new Vector2 (0.5f, 0.19f);
							curAllColliders [0].enabled = true;

							curAllColliders [1].enabled = false;

						} else {
							Debug.Log ("Ошибка. Не может быть больше 2 коллайдеров");
							return;
						}

					// Стена bottom_right --------------------------------------------------------------------------------------------------------
					} else if (grid [x, y] == floor_WALL_bottom_right) {

						spriteRenderer.sprite = floorWALL_bottom_right;
						spriteRenderer.sortingLayerName = "top_walls";

						curAllColliders = curObject.GetComponents<BoxCollider2D> ();									// Пытаемся получить список его BoxCollider2D

						if (curAllColliders.Length == 0) {

							//Debug.Log ("Создали коллайдер");
							BoxCollider2D tileBC2D = curObject.AddComponent<BoxCollider2D> ();

							tileBC2D.size = wcSize_medium_horiz;
							tileBC2D.offset = new Vector2 (0.5f, 0.19f);

							tileBC2D = curObject.AddComponent<BoxCollider2D> ();

							tileBC2D.size = wcSize_medium_vert;
							tileBC2D.offset = new Vector2 (0.81f, 0.5f);

						} else if (curAllColliders.Length == 1) {

							//Debug.Log ("Нашли 1 коллайдер");

							curAllColliders [0].size = wcSize_medium_horiz;
							curAllColliders [0].offset = new Vector2 (0.5f, 0.19f);
							curAllColliders [0].enabled = true;

							BoxCollider2D tileBC2D = curObject.AddComponent<BoxCollider2D> ();

							tileBC2D.size = wcSize_medium_vert;
							tileBC2D.offset = new Vector2 (0.81f, 0.5f);

						} else if (curAllColliders.Length == 2) {

							//Debug.Log ("Нашли 2 коллайдера");

							curAllColliders [0].size = wcSize_medium_horiz;
							curAllColliders [0].offset = new Vector2 (0.5f, 0.19f);
							curAllColliders [0].enabled = true;

							curAllColliders [1].size = wcSize_medium_vert;
							curAllColliders [1].offset = new Vector2 (0.81f, 0.5f);
							curAllColliders [1].enabled = true;

						} else {
							Debug.Log ("Ошибка. Не может быть больше 2 коллайдеров");
							return;
						}


					// Стена corner_top_left --------------------------------------------------------------------------------------------------------
					} else if (grid [x, y] == floor_WALL_corner_top_left) {

						spriteRenderer.sprite = floorWALL_corner_top_left;
						spriteRenderer.sortingLayerName = "top_walls";

						curAllColliders = curObject.GetComponents<BoxCollider2D> ();									// Пытаемся получить список его BoxCollider2D

						if (curAllColliders.Length == 0) {

							//Debug.Log ("Создали коллайдер");
							BoxCollider2D tileBC2D = curObject.AddComponent<BoxCollider2D> ();

							tileBC2D.size = wcSize_small;
							tileBC2D.offset = new Vector2 (0.81f, 0.19f);

						} else if (curAllColliders.Length == 1) {

							//Debug.Log ("Нашли 1 коллайдер");

							curAllColliders [0].size = wcSize_small;
							curAllColliders [0].offset = new Vector2 (0.81f, 0.19f);
							curAllColliders [0].enabled = true;

						} else if (curAllColliders.Length == 2) {

							//Debug.Log ("Нашли 2 коллайдера");

							curAllColliders [0].size = wcSize_small;
							curAllColliders [0].offset = new Vector2 (0.81f, 0.19f);
							curAllColliders [0].enabled = true;

							curAllColliders [1].enabled = false;

						} else {
							Debug.Log ("Ошибка. Не может быть больше 2 коллайдеров");
							return;
						}


					// Стена corner_top_right --------------------------------------------------------------------------------------------------------
					} else if (grid [x, y] == floor_WALL_corner_top_right) {

						spriteRenderer.sprite = floorWALL_corner_top_right;
						spriteRenderer.sortingLayerName = "top_walls";

						curAllColliders = curObject.GetComponents<BoxCollider2D> ();									// Пытаемся получить список его BoxCollider2D

						if (curAllColliders.Length == 0) {

							//Debug.Log ("Создали коллайдер");
							BoxCollider2D tileBC2D = curObject.AddComponent<BoxCollider2D> ();

							tileBC2D.size = wcSize_small;
							tileBC2D.offset = new Vector2 (0.19f, 0.19f);

						} else if (curAllColliders.Length == 1) {

							//Debug.Log ("Нашли 1 коллайдер");

							curAllColliders [0].size = wcSize_small;
							curAllColliders [0].offset = new Vector2 (0.19f, 0.19f);
							curAllColliders [0].enabled = true;

						} else if (curAllColliders.Length == 2) {

							//Debug.Log ("Нашли 2 коллайдера");

							curAllColliders [0].size = wcSize_small;
							curAllColliders [0].offset = new Vector2 (0.19f, 0.19f);
							curAllColliders [0].enabled = true;

							curAllColliders [1].enabled = false;

						} else {
							Debug.Log ("Ошибка. Не может быть больше 2 коллайдеров");
							return;
						}


					// Стена corner_bottom_left --------------------------------------------------------------------------------------------------------
					} else if (grid [x, y] == floor_WALL_corner_bottom_left) {

						spriteRenderer.sprite = floorWALL_corner_bottom_left;
						spriteRenderer.sortingLayerName = "default";

						curAllColliders = curObject.GetComponents<BoxCollider2D> ();									// Пытаемся получить список его BoxCollider2D

						if (curAllColliders.Length == 0) {

							//Debug.Log ("Создали коллайдер");
							BoxCollider2D tileBC2D = curObject.AddComponent<BoxCollider2D> ();

							tileBC2D.size = wcSize_small;
							tileBC2D.offset = new Vector2 (0.81f, 0.81f);

						} else if (curAllColliders.Length == 1) {

							//Debug.Log ("Нашли 1 коллайдер");

							curAllColliders [0].size = wcSize_small;
							curAllColliders [0].offset = new Vector2 (0.81f, 0.81f);
							curAllColliders [0].enabled = true;

						} else if (curAllColliders.Length == 2) {

							//Debug.Log ("Нашли 2 коллайдера");

							curAllColliders [0].size = wcSize_small;
							curAllColliders [0].offset = new Vector2 (0.81f, 0.81f);
							curAllColliders [0].enabled = true;

							curAllColliders [1].enabled = false;

						} else {
							Debug.Log ("Ошибка. Не может быть больше 2 коллайдеров");
							return;
						}


					// Стена corner_bottom_right --------------------------------------------------------------------------------------------------------
					} else if (grid [x, y] == floor_WALL_corner_bottom_right) {

						spriteRenderer.sprite = floorWALL_corner_bottom_right;
						spriteRenderer.sortingLayerName = "default";

						curAllColliders = curObject.GetComponents<BoxCollider2D> ();									// Пытаемся получить список его BoxCollider2D

						if (curAllColliders.Length == 0) {

							//Debug.Log ("Создали коллайдер");
							BoxCollider2D tileBC2D = curObject.AddComponent<BoxCollider2D> ();

							tileBC2D.size = wcSize_small;
							tileBC2D.offset = new Vector2 (0.19f, 0.81f);

						} else if (curAllColliders.Length == 1) {

							//Debug.Log ("Нашли 1 коллайдер");

							curAllColliders [0].size = wcSize_small;
							curAllColliders [0].offset = new Vector2 (0.19f, 0.81f);
							curAllColliders [0].enabled = true;

						} else if (curAllColliders.Length == 2) {

							//Debug.Log ("Нашли 2 коллайдера");

							curAllColliders [0].size = wcSize_small;
							curAllColliders [0].offset = new Vector2 (0.19f, 0.81f);
							curAllColliders [0].enabled = true;

							curAllColliders [1].enabled = false;

						} else {
							Debug.Log ("Ошибка. Не может быть больше 2 коллайдеров");
							return;
						}


					// Пол --------------------------------------------------------------------------------------------------------
					} else if (grid [x, y] == floor_01) {
						
						spriteRenderer.sprite = floor01;
						spriteRenderer.sortingLayerName = "default";

						curAllColliders = curObject.GetComponents<BoxCollider2D> ();								// Пытаемся получить его BoxCollider2D

						if (curAllColliders.Length > 0)
							foreach (BoxCollider2D tileBC2D in curAllColliders) {

								Destroy (tileBC2D);																			// Если нашли BoxCollider2D, то удаляем
								//Debug.Log ("Удалили коллайдер");

							}
					}

				} else {
					
					Debug.Log (string.Format ("Ошибка. Не найден объект: '{0}'", curObject));

					return;
				}

			}

	}

	// Use this for initialization
	void Start () {

		Random.seed = (int)System.DateTime.Now.Ticks;

		grid = new int[levelWidth, levelHeight];
		gridGObjects = new GameObject [levelWidth, levelHeight];
		gridSpriteRenderers = new SpriteRenderer[levelWidth, levelHeight];

		wcSize_full = new Vector2 (1f, 1f);
		wcSize_medium_vert = new Vector2 (0.384f, 1f);
		wcSize_medium_horiz = new Vector2 (1f, 0.384f);
		wcSize_small = new Vector2 (0.384f, 0.384f);

		for (int y = 0; y < levelHeight; y++)
			for (int x = 0; x < levelWidth; x++) {

				GameObject tile = new GameObject ();											// Создаем объект

				tile.transform.position = new Vector3 (x, y, 0f);								// Присваеваем ему координаты

				gridSpriteRenderers[x, y] = tile.AddComponent<SpriteRenderer>();				// Добавляем в массив рендер спрайта

				//BoxCollider2D wallCollider =  gridGObjects[x, y].AddComponent<BoxCollider2D>();

				tile.name = "Floor_" + x + "_" + y;												// Имя спрайта (название + позиция)
				tile.transform.parent = transform;												// Прописываем родителя

				gridGObjects[x, y] = tile;
			}

		CreateRandomLevel ();																	// Создаем уровень

	}

	// Update is called once per frame
	void Update () {

		if (Input.GetKeyUp (KeyCode.Space) && keySpaceUpPressed == 0) {

			keySpaceUpPressed = 1;
			//Debug.Log (string.Format ("keySpaceUpPressed = {0}", keySpaceUpPressed));

		}

		if (Input.GetKey (KeyCode.Space) && keySpaceUpPressed == 1) {
			CreateRandomLevel ();

			keySpaceUpPressed = 0;
			//Debug.Log (string.Format ("keySpaceUpPressed = {0}", keySpaceUpPressed));


		}

	}
}
