# OGL
Na podstawie książki i dodatku K. Żydzika: C# 6.0 i MVC 5.0 Tworzenie nowoczesnych portali internetowych

Wzorce projektowe i architektoniczne wykorzystywane w .NET 
* Repozytorium - operacje korzystające z bazy danych są przenoszone do innej warstwy w aplikacji (osobnego projektu). W kontrolerze wywołuje się tylko metody z repozytorium. Z kilku kontrolerów można wywoływać tę samą metodę bez konieczności kopiowania lub ponownego pisania. Kontroler nie odpowiada za utworzenie kontekstu (obiektu dostępu do danych) i nie posiada informacji na temat tego, skąd są dane ani jaką strukturę ma baza danych. 
* Wzorzec IoC - Repozytorium ma jeden duży minus — instancja repozytorium jest tworzona w kontrolerze, co powoduje, że każdy kontroler jest zależny nie od kontekstu, a od repozytorium. Aby tworzenie obiektu repozytorium było niezależne od kontrolera, wykorzystuje się wzorzec IoC (ang. Inversion of Control). Dzięki takiemu rozwiązaniu instancja repozytorium jest wstrzykiwana poprzez konstruktor. IoC pozwala na działanie na interfejsie zamiast na określonej klasie. Dzięki temu można w łatwy sposób podmienić implementację repozytorium. W konfiguracji biblioteki odpowiedzialnej za IoC wybiera się, jaka klasa ma być wstrzyknięta dla danego interfejsu. Może być na przykład kilka wersji tego samego repozytorium. 
* Repozytorium generyczne - Bez względu na rodzaj klasy , na której się działa, istnieje kilka podstawowych metod wykorzystywanych w przypadku większości klas. Takimi metodami są na przykład operacje dodawania lub usuwania. Aby nie pisać tych samych deklaracji metod w każdym repozytorium, należy utworzyć generyczny interfejs repozytorium, po którym będą dziedziczyć wszystkie pozostałe interfejsy lub repozytoria. Dzięki repozytorium generycznemu nie ma potrzeby przepisywania tych samych metod do różnych interfejsów lub klas. Według niektórych wzorzec ten jest uznawany za antywzorzec. 
* Wzorzec UnitOfWork - pozwala na grupowanie kilku zadań z różnych repozytoriów w jedną transakcję. Służy do dzielenia pojedynczego kontekstu pomiędzy różne repozytoria. UnitOfWork znajduje zastosowanie, gdy chce się wprowadzić zmiany w wielu tabelach za pomocą wielu repozytoriów na pojedynczym kontekście. W przypadku bardzo wielu pojedynczych, prostych zapytań do bazy danych z różnych repozytoriów tworzone są osobne konteksty , a każde zapytanie jest wysyłane jako pojedyncze żądanie. Korzystając z UnitOfWork, oszczędza się zasoby serwera, ponieważ nie trzeba tworzyć wielu osobnych połączeń z bazą ani wielu kontekstów. Wszystkie operacje korzystają z pojedynczego kontekstu i pojedynczego połączenia.


## Etap 1. Krok 1. Tworzenie nowego projektu i aktualizacja pakietów 
* nazwa projektu i solucji: OGL, wybrany template: MVC, referencje do MVC i WebApi, zaznaczono Add Unit Tests: Ogl.Tests, pooświadczenie: Indywidual User Account
* aktualizacja bibliotek Manage NuGet Packages i restart aplikacji

## Etap 1. Krok 2. Utworzenie modelu danych w podejściu Code First
* folder Models mam utworzony 
* dodanie do niego klasy Ogloszenie, 
* wypełnienie ciała klasy: 
	- relacja 1-* (wiele ogłoszeń do jednego użytkownika) 
	```
	public string UzytkownikId { get; set; }
	public virtual Uzytkownik Uzytkownik { get; set; }
	```
	- oraz wiele ogłoszeń - wiele kategorii: wymaga dodatkowej klasa Ogloszenie_Kategoria dla której mamy relację 1-*
	```
	public virtual ICollection<Ogloszenie_Kategoria>Ogloszenie_Kategoria { get; set; }
	```	
	- dodanie referencji: using System.ComponentModel.DataAnnotations;
* dodanie klasy Kategoria:
	- relacja wiele ogłoszeń - wiele kategorii: do klasy Ogloszenie_Kategoria mamy relację 1-*
	```
	public Kategoria()    
	{   
		this.Ogloszenie_Kategoria = new HashSet<Ogloszenie_Kategoria>();    
	}
	public ICollection<Ogloszenie_Kategoria> Ogloszenie_Kategoria { get; set; }
	```
* dodanie klasy Ogloszenie_Kategoria
```
public Ogloszenie_Kategoria()    {    }    
public int KategoriaId {get; set;}    
public int OgloszenieId { get; set; }    
public virtual Kategoria Kategoria { get; set; }    
public virtual Ogloszenie Ogloszenie {get; set;} 
```
* klasa Uzytkownik
	- zmiana nazwy klasy ApplicationUser na Uzytkownik.
	- relacja 1-* (jednego użytkownika do wielu ogłoszeń) 
	```
	public Uzytkownik()    
	{        
		this.Ogloszenia = new HashSet<Ogloszenie>();    
	} 
	public virtual ICollection<Ogloszenie> Ogloszenia { get; private set; } 
	```

## Etap 1. Krok 3. Tworzenie klasy kontekstu 
* w folderze Models zmieniamy nazwę pliku z IdentityModels na OglContext. Po zmianie nazwy pliku otwórz plik kontekstu (teraz już o nazwie OglContext) i zmień nazwę klasy z ApplicationDbContext na OglContext (razem z konstruktorem i statyczną metodą) 
Dodatkowo zamień: IdentityDbContext<Uzytkownik> na: IdentityDbContext  usuń: , throwIfV1Schema: false<br/>
Kod klasy po dokonaniu zmian:<br/>
```
public class OglContext : IdentityDbContext {    
	public OglContext()  : base("DefaultConnection")    {    }    
	public static OglContext Create()    {        
		return new OglContext();    
	} 
}
```

* Teraz w całej aplikacji trzeba zamienić ApplicationDbContext na OglContext
* Jeśli nie chcesz korzystać z gotowej funkcjonalności lub nie musisz uwierzytelniać użytkowników, możesz utworzyć 	klasę dziedziczącą po DbContext: 	
```
public class OglContext : DbContext {}  
```
Korzystamy tu z gotowej funkcjonalności lub chcę uwierzytelniać użytkowników, więc nie korzystam z DbContext.

* Teraz dodaj do niej właściwości DbSet reprezentujące w pamięci kontekstu tabele z bazy danych. Oto kod:
```
public class OglContext : DbContext {    
	public OglContext()        : base("DefaultConnection")    {    }    
	public static OglContext Create()    {        
		return new OglContext();    
	}    
	public DbSet<Kategoria> Kategorie { get; set; }    
	public DbSet<Ogloszenie> Ogloszenia { get; set; }    
	public DbSet<Uzytkownik> Uzytkownik { get; set; }    
	public DbSet<Ogloszenie_Kategoria> Ogloszenie_Kategoria { get; set; } 
} 
```

*  Aby aplikacja nie zmieniała nazw, trzeba nadpisać konwencję. Jeśli chcesz to zrobić dla kontekstu, skorzystaj z metody: 
```
modelBuilder.Conventions.Remove<PluralizingTableNameConvention>(); 
```

* Następnie dla relacji Ogloszenie – Uzytkownik włączono CascadeDelete za pomocą Fluent API. 	Oto kod:
```
protected override void OnModelCreating(DbModelBuilder modelBuilder) {    
	// Potrzebne dla klas Identity    
	base.OnModelCreating(modelBuilder);    
	// using System.Data.Entity.ModelConfiguration.Conventions;    
	// Wyłącza konwencję, która automatycznie tworzy liczbę mnogą dla nazw tabel w bazie danych    
	// Zamiast Kategorie zostałaby utworzona tabela o nazwie Kategories    
	modelBuilder.Conventions.Remove <PluralizingTableNameConvention>();    
	// Wyłącza konwencję CascadeDelete    
	// CascadeDelete zostanie włączone za pomocą Fluent API    
	modelBuilder.Conventions.Remove <OneToManyCascadeDeleteConvention>();
	// Używa się Fluent API, aby ustalić powiązanie pomiędzy tabelami  
	// i włączyć CascadeDelete dla tego powiązania    
	modelBuilder.Entity<Ogloszenie>().HasRequired(x => x.Uzytkownik).WithMany(x => x.Ogloszenia) .HasForeignKey(x => x.UzytkownikId) .WillCascadeOnDelete(true); 
} 
```

*  W folderze Models znajdują się jeszcze pliki: AccountViewModels i ManageViewModels. Są to klasy z ViewModel na potrzeby logowania przez zewnętrzne serwisy i do zarządzania kontem użytkownika. Należy utworzyć folder Views w folderze Models i przenieść tam obydwa pliki. 

## Etap 1. Krok 4. Przenoszenie warstwy modelu do osobnego projektu 		
* Utwórz mowy projekt ASP .NET Web Application i nazwij projekt Repozytorium. Wybierz template: Empty
* Dodawanie referencji pomiędzy projektami:  Dodaj referencję do Repozytorium z projektu OGL, kliknij prawym przyciskiem myszy References w projekcie OGL (rysunek 8.24) i wybierz Add Reference: Repozytorium. 
* Ustawienie projektu startowego na OGL
* Instalacja bibliotek dla projektu Repozytorium: Entity Framework, Microsoft ASP.NET MVC, Microsoft ASP.NET Identity Framework 
* Przenoszenie plików z modelem do osobnej warstwy (projektu) z OGL do Repozytorium, poza ManageViewModels.cs ponieważ posiada on referencje do Microsoft.Owin.Security, a nie chcemy instalować pakietu OWIN w projekcie Repozytorium. 
* Zmień nazwy w aplikacji z: OGL.Models na: Repozytorium.Models
* i Rebuild Solution
	
## Etap 1. Krok 5. Migracje
* Instalacja migracji: W oknie Package Manager Console wybieramy projekt Repozytorium i wpisujemy Enable-Migration. Zostanie utworzony folder Migrations, a w nim plik konfiguracyjny Configuration
* Konfiguracja migracji: Aby włączyć automatyczne migracje oraz włączyć migracje stratne, dodaj następujący kod w konstruktorze klasy konfiguracyjnej: 
```
public Configuration() {    
	AutomaticMigrationsEnabled = true;    
	AutomaticMigrationDataLossAllowed = true; 
} 
```
Od teraz bez pytania po zmianach w klasach z modelem będą wykonywane również migracje stratne, po czym zostanie wykonana metoda Seed. 

* Tworzenie migracji początkowej: 	`Add-migration startowa `
* Uruchomienie pierwszej migracji:	`Update-Database`
* W Web.config zmianiam nazwę do bazy danych z pliku .mdf na bazę Sql Server następująco: 
```
connectionString="Data Source= (LocalDb)\v11.0;AttachDbFilename=|DataDirectory|\aspnet-OGL20140807102454.mdf;Initial Catalog=aspnet-OGL-20140807102454;Integrated Security=True" 
```

* Ponownie uruchomiemy migrację: `Update-Database`
* Uzupelniam metodę Seed()
* Debugowanie metody Seed: tylko jeśli dodamy wpis na początku metody Seed(). Po debugowaniu zakomentuj wpis:
```
if (System.Diagnostics.Debugger.IsAttached == false)    
	System.Diagnostics.Debugger.Launch(); 
```

* Zmiany w modelu i kolejna migracja: Dodaj pole Wiek do klasy Uzytkownik:
```
public int Wiek { get; set; }
```
Zapisz plik i uruchom komendę: 	`Add-migration 1`	gdzie 1 to nazwa migracji. 

* Aby było możliwe przypisanie wartości null, pole Wiek należy oznaczyć jako Nullable i uruchomić migrację	
```
public int? Wiek { get; set; }
```

* W SeedUsers() kod 
```
var user = new Uzytkownik { UserName = "Admin" };
```
należy zastąpić następującym (aby przepisać wiek użytkownikowi): 
```
var user = new Uzytkownik { UserName = "Admin", Wiek = 12 };
```
	

## Etap 2. Krok 1. Dodawanie kontrolerów i widoków — akcja Index
* Dodawanie kontrolera z widokami przez: MVC 5 Controller with views, using Entity Framework
		Model: Ogloszenie, Context: OglContext, Nazwa: OgloszenieControlle.
		Zaznacz: Generate Views, Reference script libraries i Use a layout page
* Lista ogłoszeń (akcja Index) — aktualizacja widoku/wyglądu strony 
	- Zmiany:
	```
	<h2>Index</h2> 	na:
	<h2>Lista ogłoszeń</h2>
	@Html.DisplayNameFor(model => model.Uzytkownik.Email)	na  
	@Html.DisplayNameFor(model => model.Uzytkownik.UserName)
	@Html.ActionLink("Edit", "Edit", new { id=item.Id }) 	 na: 
	@Html.ActionLink("Edytuj", 	  "Edit", 	new { id=item.Id })
	@Html.ActionLink("Szczegóły", "Details",new { id=item.Id }) |            
	@Html.ActionLink("Usuń", 	   "Delete", new { id=item.Id })
	```
	- Lista ogłoszeń a pobieranie danych 
	```
	public ActionResult Index() {    
		var ogloszenia = db.Ogloszenia.Include(o => o.Uzytkownik);    
		return View(ogloszenia.ToList()); 
	}
	```
Za pomocą metody Include() dla każdego ogłoszenia ładowany jest użytkownik. Aby sprawdzić, jakie zapytanie zostało wysłane do bazy danych, skorzystano z nowości wprowadzonej w EF6, a więc logowania SQL. Przerwij działanie programu i na początku akcji Index dodaj linijkę: 
	```
	db.Database.Log = message => Trace.WriteLine(message);
	```

Zaimportuj bibliotekę (using System.Diagnostics;) dla podkreślonego kodu. Uruchom aplikację (F5) i zobacz zapytanie Sql w oknie Output.
	- Include() - włacza LazyLoading, dla każdego ogłoszenia ładowany jest użytkownik
	- AsNoTracking() - wyłączy śledzenie danych przez kontekst

	
## Etap 2. Krok 2. Debugowanie oraz metody AsNoTracking() i ToList()	

## Etap 2. Krok 3. Poprawa wyglądu i optymalizacja pod kątem SEO 
* Poprawa wyglądu strony za pomocą Twitter Bootstrap (Ogloszenie/Index)
```
@Html.ActionLink("Dodaj nowe ogłoszenie", "Create") na: 
@Html.ActionLink("Dodaj nowe ogłoszenie", "Create", null, new { @class = "btn btn-primary" }) 
@Html.ActionLink("Szczegóły", "Details", new { id = item.Id }, new { @class = "btn btn-warning" })    <br />         
@Html.ActionLink("Edytuj ", "Edit", new { id = item.Id },new { @class = "btn btn-primary" })    <br />    
@Html.ActionLink("Usuń", "Delete", new { id = item.Id }, new { @class = "btn btndanger" })
```

* Site.css: Podświetlanie wierszy za pomocą CSS oraz dodanie odstępów pomiędzy przyciskami
```
tr:first-child{	background-color:#efefef; }
tr:hover td{    background-color:#efefef; } 
.btn {    margin:2px; }
```

* Optymalizacja pod kątem pozycjonowania — SEO: tytuł, metaopis i słowa kluczowe
	- _Layout.cshtml - dodaj pola description i keywords, ponieważ znacznik title został dodany domyślnie
	```
	<title>@ViewBag.Tytul</title>
	<meta name="description" content="@ViewBag.Opis" />
	<meta name="keywords" content="@ViewBag.SlowaKluczowe" />
	```

	- view np /Ogloszenie/Index
	```
	@model IEnumerable<Repozytorium.Models.Ogloszenie> 
	@{    
		ViewBag.Tytul = "Lista ogłoszeń - metatytuł do 60 znaków";    
		ViewBag.Opis = "Lista ogłoszeń z naszej aplikacji — metaopis do 160 znaków";    
		ViewBag.SlowaKluczowe = "Lista, ogłoszeń, słowa, kluczowe, aplikacja"; 
	}
	```


## Etap 3. Krok 1. Poprawa architektury aplikacji
* Przeniesienie zapytania LINQ do osobnej metody
```
public ActionResult Index() {    
	var ogloszenia = db.Ogloszenia.AsNoTracking();    
	return View(ogloszenia); 
} 
```

Następnie przenieś zapytanie oraz funkcję odpowiedzialną za logowanie zapytań do osobnej metody o nazwie PobierzOgloszenia(), która będzie zwracać typ IQueryable<Ogloszenie>. Kod po przenosinach wygląda następująco: 
```
public ActionResult Index() {    
	var ogloszenia = PobierzOgloszenia();    
	return View(ogloszenia); 
} 
public IQueryable<Ogloszenie> PobierzOgloszenia() {    
	db.Database.Log = message => Trace.WriteLine(message);    
	return db.Ogloszenia.AsNoTracking(); 
}
```			

* Przeniesienie metody do repozytorium
	- dodaj folder o nazwie Repo
	- dodaj do niej klasę OgloszenieRepo
	```
	public class OgloszenieRepo {    
		private OglContext db = new OglContext();    
		public IQueryable<Ogloszenie> PobierzOgloszenia()    {        
			db.Database.Log = message => Trace.WriteLine(message);        
			return db.Ogloszenia.AsNoTracking();    
		} 
	}
	```

	- zmien klasę OgloszenieController
	```
	public class OgloszenieController : Controller {    
		OgloszenieRepo repo = new OgloszenieRepo();     
		// GET: /Ogloszenie/    
		public ActionResult Index()    {        
			var ogloszenia = repo.PobierzOgloszenia();        
			return View(ogloszenia);    
		}    
		// Tutaj zakomentowany kod/akcje 
	} 
	```

## Etap 3. Krok 2. Zastosowanie kontenera Unity — IoC 
* Wstrzykiwanie repozytorium poprzez konstruktor w kontrolerze
```
public class OgloszenieController : Controller {    
	private readonly IOgloszenieRepo _repo;    
	public OgloszenieController(IOgloszenieRepo repo)    {        
		_repo = repo;    
	}    
	// GET: /Ogloszenie/    
	public ActionResult Index()    {        
		var ogloszenia = _repo.PobierzOgloszenia();        
		return View(ogloszenia);    
	}    
	// Tutaj zakomentowany kod/akcje 
}
```

* Tworzenie interfejsu dla repozytorium
	- dodaj folder o nazwie IRepo
	- a w nim interfejs IOgloszenieRepo
	```
	public interface IOgloszenieRepo {    
		IQueryable<Ogloszenie> PobierzOgloszenia(); 
	}
	```

	- Repozytorium OgloszenieRepo musi dziedziczyć po interfejsie IOgloszenieRepo i implementować jego składowe
	```
	public class OgloszenieRepo : IOgloszenieRepo {    
		private OglContext db = new OglContext();    
		public IQueryable<Ogloszenie> PobierzOgloszenia()    {        
			db.Database.Log = message => Trace.WriteLine(message);
			var ogloszenia = db.Ogloszenia.AsNoTracking();
			return ogloszenia;    
		} 
	}
	```

* Instalacja kontenera IoC Unity: Unity.Mvc. Po zainstalowaniu biblioteki w klasie UnityConfig w folderze App_Start dopisz następujący kod do metody RegisterTypes():
```
public class UnityConfig {    
	#region Unity Container 
	...
	#endregion   
	public static void RegisterTypes(IUnityContainer container)    {
		container.RegisterType<AccountController>(new InjectionConstructor());
		container.RegisterType<ManageController>(new InjectionConstructor());
		container.RegisterType<IOgloszenieRepo, OgloszenieRepo>(new PerRequestLifetimeManager());
} 
```	

* Wstrzykiwanie kontekstu do repozytorium: 
	- utwórz interfejs IOglContext w folderze IRep.
	```
	public interface IOglContext {    
		DbSet<Kategoria> Kategorie { get; set; }
		DbSet<Ogloszenie> Ogloszenia { get; set; }    
		DbSet<Uzytkownik> Uzytkownik { get; set; }    
		DbSet<Ogloszenie_Kategoria> Ogloszenie_Kategoria { get; set; }    
		int SaveChanges();    
		Database Database { get; } 
	} 
	```

	- Przejdź do pliku OglContext i zamień pierwszą linię z: 'public class OglContext : IdentityDbContext' na: 
	```
	public class OglContext : IdentityDbContext, IOglContext
	```

	- Przejdź teraz do OgloszenieRepo i utwórz konstruktor, w którym będzie wstrzykiwana instancja repozytorium. Kod:
	```
	private readonly IOglContext _db; 
	public OgloszenieRepo(IOglContext db) {    
		_db = db; 
	} 
	public IQueryable<Ogloszenie> PobierzOgloszenia() {    
		_db.Database.Log = message => Trace.WriteLine(message);    
		var ogloszenia = _db.Ogloszenia.AsNoTracking();    
		return ogloszenia; 
	} 
	```

	- skonfigurowanie kontenera Unity, aby wstrzykiwał w miejsce interfejsu IOglContext instancję klasy OglContext. Po dodaniu metoda wygląda następująco: 
	```
	public static void RegisterTypes(IUnityContainer container) {    	
		container.RegisterType<AccountController>(new InjectionConstructor());
		container.RegisterType<ManageController>(new InjectionConstructor());
		container.RegisterType<IOgloszenieRepo, OgloszenieRepo>(new PerRequestLifetimeManager());
		container.RegisterType<IOglContext, OglContext>(new PerRequestLifetimeManager()); 
	} 
	```
	
	- Cykl życia obiektu a kontener IoC. W aplikacjach internetowych dane są zazwyczaj pobierane tylko jeden raz i nie ma potrzeby przechowywania obiektu w pamięci po zakończonym żądaniu. Dlatego w metodzie RegisterTypes() ustawiono cykl życia obiektu jako PerRequest zarówno dla kontekstu, jak i repozytorium. Oznacza to, że przy każdym żądaniu zostanie utworzony nowy obiekt, a po zakończeniu żądania kontener IoC automatycznie wywoła metodę `Dispose()`: 
	```
	protected override void Dispose(bool disposing) 
	{    
		if (disposing)    {        
			db.Dispose();    
		}    
		base.Dispose(disposing); 
	}
	```
	

## Etap 4. Krok 1. Akcje Details, Create, Edit, Delete dla kontrolera OgloszenieController 
* Details - odkomentuj akcję Details
```
public ActionResult Details(int? id) {    
	if (id == null)    {        
		return new HttpStatusCodeResult(HttpStatusCode.BadRequest);    
	}    
	Ogloszenie ogloszenie = db.Ogloszenia.Find(id);    
	if (ogloszenie == null)    {        
		return HttpNotFound();    
	}    
	return View(ogloszenie); 
} 
```

* Metoda Details() w repozytorium 
	- Utwórz metodę w repozytorium, która pobierze ogłoszenie. Dodaj zatem w interfejsie IOgloszenieRepo następującą deklarację metody: 
	```
	Ogloszenie GetOgloszenieById(int id); 
	```
	
	- Kolejnym krokiem jest implementacja metody GetOgloszenieById w repozytorium OgloszenieRepo. Dodaj do repozytorium OgloszenieRepo następującą metodę: 
	```
	public Ogloszenie GetOgloszenieById(int id) {    
		Ogloszenie ogloszenie = _db.Ogloszenia.Find(id);    
		return ogloszenie; 
	}
	```
	
	- Ostatnim krokiem jest wykorzystanie w kontrolerze nowo dodanej metody . Zamień linijkę: 
	```
	Ogloszenie ogloszenie = db.Ogloszenia.Find(id); 
	``` 
	na: 
	```
	Ogloszenie ogloszenie = _repo.GetOgloszenieById((int)id);
	```
	
* Aktualizacja i optymalizacja SEO dla widoku Details 
	- Po uruchomieniu aplikacji kliknij przycisk Szczegóły na podstronie z listą ogłoszeń (rysunek 8.82).
	- Popraw wygląd aplikacji, tak jak zostało to zrobione dla akcji Index (rysunek 8.83, listing 8.7): 
		- ustaw tytuł, opis i słowa kluczowe, 
		- przetłumacz linki, 
		- dodaj kolorowe przyciski za pomocą Bootstrapa, 
		- wyświetl identyfikator użytkownika w miejsce e-maila.
		
* Delete
	- Odkomentuj metodę Delete
	```
	public ActionResult Delete(int? id) {    
		if (id == null)    
		{        
			return new HttpStatusCodeResult(HttpStatusCode.BadRequest);    
		}    
		Ogloszenie ogloszenie = _repo.GetOgloszenieById((int)id);    
		if (ogloszenie == null)    {        
			return HttpNotFound();    
		}    
		return View(ogloszenie); 
	}
	```

* Aktualizacja widoku dla akcji Delete 
	- Kolejnym krokiem jest poprawienie pliku z widokiem Delete (rysunek 8.84, listing 8.8). Ponownie wprowadź te same zmiany co w widoku Details, czyli: 
		- ustaw tytuł, opis i słowa kluczowe, 
		- przetłumacz linki, 
		- dodaj kolorowe przyciski za pomocą Bootstrapa, 
		- wyświetl identyfikator użytkownika w miejsce e-maila. 
	
* Metoda POST dla akcji Delete 
	- Odkomentuj metodę DeleteConfirmed
	```
	[HttpPost, ActionName("Delete")] 
	[ValidateAntiForgeryToken] 
	public ActionResult DeleteConfirmed(int id) 
	{    
		Ogloszenie ogloszenie = db.Ogloszenia.Find(id);    
		db.Ogloszenia.Remove(ogloszenie);    
		db.SaveChanges();    
		return RedirectToAction("Index"); 
	} 
	```
	
	- Przenieś teraz kod odpowiedzialny za usuwanie ogłoszenia do repozytorium. Utwórz deklarację metody w interfejsie IOgloszenieRepo: 
	```
	bool UsunOgloszenie(int id); 
	```
	
	Metoda będzie zwracać wartość true, jeśli usuwanie zakończy się sukcesem, lub false, gdy wystąpi problem podczas usuwania. Poniżej znajduje się implementacja metody UsunOgloszenie(), którą należy dodać do repozytorium OgloszenieRepo: 
	```
	public bool UsunOgloszenie(int id) 
	{    
		Ogloszenie ogloszenie = _db.Ogloszenia.Find(id);    
		_db.Ogloszenia.Remove(ogloszenie);    
		try    {        
			_db.SaveChanges();        
			return true;    
		}    
		catch(Exception ex)    
		{        
			return false;    
		} 
	} 
	```
	
	Metoda UsunOgloszenie() zwróci wartość true, jeśli zapis do bazy się powiedzie, lub wartość false w przeciwnym wypadku. 
	
* Metoda Delete() w repozytorium - należy zmodyfikować metodę `DeleteConfirmed()` z kontrolera `OgloszenieController`, aby korzystała z metody znajdującej się w repozytorium
	```
	// POST: /Ogloszenie/Delete/5 
	[HttpPost, ActionName("Delete")] 
	[ValidateAntiForgeryToken] 
	public ActionResult DeleteConfirmed(int id) 
	{    
		for (int i = 0; i < 3; i++)    
		{        
			if (_repo.UsunOgloszenie(id))            
			break;
		}    
		return RedirectToAction("Index"); 
	}
	```
	
* Kaskadowe usuwanie i błędy - Uruchomiliśmy aplikację, przeprowadziliśmy próbę usunięcia ogłoszenia, ale aplikacja wraca do akcji Index, a ogłoszenie nie zostaje usunięte. Podczas debugowania można zauważyć, że w bloku catch() w metodzie UsunOgloszenie z OgloszenieRepo zgłaszany jest wyjątek. 
Dzieje się tak, ponieważ ogłoszenie jest powiązane z kategoriami poprzez tabele Ogloszenie_Kategoria. Aby usunięcie ogłoszenia było możliwe, konieczne jest wcześniejsze usunięcie wpisów z tabeli Ogloszenie_Kategoria, które w kluczu obcym OgloszenieId są powiązane z usuwanym ogłoszeniem. Zatem w akcji Delete zostaną usunięte wszystkie wiersze z tabeli Ogloszenie_Kategoria, dla których OgloszenieId jest równe id ogłoszenia, a dopiero później będzie wykasowane również ogłoszenie. Jeśli w klasie kontekstu ustawiłbyś kaskadowe usuwanie (ang. Cascade Delete) na true, to nie byłoby konieczności pisania dodatkowej metody i baza danych automatycznie usunęłaby dane z tabeli Ogloszenie_Kategoria. 
	- Dodaj jeszcze następującą prywatną (dostępną tylko dla klasy repozytorium) metodę do OgloszenieRepo: 
	```
	private void UsunPowiazanieOgloszenieKategoria(int idOgloszenia) 
	{    
		var list = _db.Ogloszenie_Kategoria.Where(o=>o.OgloszenieId == idOgloszenia);    
		foreach (var el in list)    {        
			_db.Ogloszenie_Kategoria.Remove(el);    
		} 
	} 
	```
	
	i wywołaj ją na początku w metodzie UsunOgloszenie(): 
	```
	public bool UsunOgloszenie(int id) 
	{    
		UsunPowiazanieOgloszenieKategoria(id);    
		Ogloszenie ogloszenie = _db.Ogloszenia.Find(id);    
		_db.Ogloszenia.Remove(ogloszenie);    
		try    
		{        
			_db.SaveChanges();        
			return true;    
		}    
		catch(Exception ex)    
		{        
			return false;    
		} 
	} 
	```
	
	Uruchom ponownie aplikację i spróbuj usunąć ogłoszenie. Tym razem zadanie zostanie wykonane. 

* Przeniesienie metody SaveChanges() poza repozytorium 
Dodaj deklarację metody do interfejsu IOgloszenieRepo: 
```
void SaveChanges(); 
```

Do klasy OgloszenieRepo dodaj definicję metody: 
```
public void SaveChanges() 
{    
	_db.SaveChanges(); 
}
```

W metodzie UsunOgloszenie() usuń kod odpowiedzialny za zapis do bazy danych i zmień typ zwracany z metody na void. Ponieważ zapis odbywa się w kontrolerze, nie ma potrzeby zwracać informacji, czy zapis się powiódł. W interfejsie IOgloszenieRepo zmień również deklarację metody , aby zwracała typ void. Metoda `UsunOgloszenie()` po aktualizacji powinna wyglądać następująco: 
```
public void UsunOgloszenie(int id) 
{    
	UsunPowiazanieOgloszenieKategoria(id);    
	Ogloszenie ogloszenie = _db.Ogloszenia.Find(id);    
	_db.Ogloszenia.Remove(ogloszenie); 
}
```

Należy zmienić kod metody DeleteConfirmed() z kontrolera aby wyglądał jak poniżej: 
```
// POST: /Ogloszenie/Delete/5 
[HttpPost, ActionName("Delete")] 
[ValidateAntiForgeryToken] 
public ActionResult DeleteConfirmed(int id) 
{    
	_repo.UsunOgloszenie(id);    
	try    {        
		_repo.SaveChanges();    
	}    
	catch    {    }    
	return RedirectToAction("Index"); 
}
```

* Obsługa błędów w akcji Delete
Dodaj jeden opcjonalny parametr typu bool o nazwie blad. Jeśli parametr zostanie przekazany do widoku, będzie to oznaczać, że wystąpił błąd i trzeba wyświetlić komunikat o błędzie. 
```
public ActionResult Delete(int? id, bool? blad) 
{    
	if (id == null)    {        
		return new HttpStatusCodeResult(HttpStatusCode.BadRequest);    
	}    
	Ogloszenie ogloszenie = _repo.GetOgloszenieById((int)id);
    if (ogloszenie == null)    {        
		return HttpNotFound();    
	}    
	if(blad !=null)        
		ViewBag.Blad = true;    
	return View(ogloszenie); 
}
```

Teraz zajmij się widokiem. Jeśli ViewBag będzie miał wartość true, to znaczy , że trzeba wyświetlić komunikat o błędzie. Dymek z czerwonym tłem utwórz za pomocą Bootstrapa i klasy `alert alert-danger`. Dodaj następujący kod pod znacznikiem `<h2>` do widoku `Delete.cshtml`: 
```
@if (ViewBag.Blad == true) 
{    
	<div class="alert alert-danger" role="alert">    
	Wystąpił błąd podczas usuwania.<br/>    
	Spróbuj ponownie.    
	</div> 
}
```

Ostatnim krokiem (metoda POST) jest wywołanie akcji Delete z parametrem blad = true, jeśli wystąpi wyjątek. W metodzie `DeleteConfirmed()` i w bloku catch dodaj następującą linię kodu: 
```
return RedirectToAction("Delete", new {id = id, blad = true }); 
```

Ostatecznie metoda DeleteConfirmed() wygląda następująco: 
```
// POST: /Ogloszenie/Delete/5
[HttpPost, ActionName("Delete")] 
[ValidateAntiForgeryToken] 
public ActionResult DeleteConfirmed(int id) 
{    
	_repo.UsunOgloszenie(id);    
	try    {        
		_repo.SaveChanges();    
	}    
	catch    {        
		return RedirectToAction("Delete", new {id = id, blad = true });    
	}    
	return RedirectToAction("Index"); 
} 
```

Aby przetestować działanie dymka z informacją o błędzie, odwiedź adres /Ogloszenie/Delete/3?blad=True. 

* JavaScript, jQuery i okno potwierdzania wyboru 
	- Sposób 1. Do elementu <input> na stronie z widokiem Delete dodaj metodę `onclick`: 
	```
	onclick="return confirm('Czy na pewno chcesz usunąć?')" 
	```

	Linijkę: `<input type="submit" value="Usuń" class="btn btn-danger" />` zamień na: `<input type="submit" value="Usuń" class="btn btn-danger" onclick="returnconfirm('Czy na pewno chcesz usunąć?')" />` 
	Po kliknięciu przycisku Usuń pokaże się okienko z komunikatem widocznym na rysunku 8.86.

	- Sposób 2. W pliku z szablonem Layout.cshtml na samym końcu znajduje się następujący kod: 
	```
	@RenderSection("scripts", required: false) 
	</body> 
	</html>
	```
	
	Jest to kod odpowiedzialny za wygenerowanie w tym miejscu dokumentu HTML sekcji scripts, jeśli taka sekcja jest zdefiniowana w pliku z widokiem. Required: false mówi o tym, że ta sekcja jest opcjonalna i nie musi być umieszczona na każdej podstronie (w każdym pliku z widokiem). Wystarczy wkleić na samym dole pliku z widokiem `Delete` następujący kod korzystający z jQuery: 
	```
	@section Scripts{    
		<script type="text/javascript">        
			$('.confirmation').on('click', function () {            
				return confirm('Czy na pewno chcesz usunąć?');        
			});    
		</script> 
	} 
	```
	Fragment kodu `(.on('click', function () {)` jest odpowiedzialny za oczekiwanie na zdarzenie kliknięcia elementu posiadającego klasę confirmation ($('.confirmation')). Odpowiedzialny jest on również za wyświetlenie okienka z potwierdzeniem `(return confirm('Czy na pewno chcesz usunąć?');)`. Teraz w przeglądarce koniec wygenerowanego dokumentu HTML będzie wyglądał następująco: 
	```
			<script type="text/javascript">    
				$('.confirmation').on('click', function () {        
					return confirm('Czy na pewno chcesz usunąć?');    
				});
			</script> 
		</body> 
	</html> 
	```
	Aby wyświetliło się okienko z potwierdzeniem, trzeba jeszcze dodać klasę confirmation do elementu `<input>`: 
	```
	<input type="submit" value="Usuń" class="btn btn-danger confirmation" />
	```
	
	Efekt jest identyczny jak przy wykorzystaniu sposobu numer 1, ale przedstawione zostało wykorzystanie sekcji i kodu JavaScript lub jQuery . 
	
* Create - odkomentuj dwie akcje z kontrolera OgloszenieController
Kod po modyfikacji:
```
// GET: Ogloszenie/Create 
public ActionResult Create() 
{    
	return View(); 
} 
```

* Aktualizacja widoku dla akcji Create - Kolejnym krokiem jest przerobienie widoku Create. 
	- Usuń kod odpowiedzialny za pola UzytkownikId oraz DataDodania. 
	- Przetłumacz linki, 
	- zmień klasę CSS przycisku na btn-success, aby był zielony (listing 8.9, rysunek 8.87) 
	- oraz dodaj opisy do Google. 

* Metoda POST dla akcji Create 
A oto lista zmian, które należy wprowadzić (listing 8.10): 
	- automatyczne przypisanie Id użytkownika, który dodaje ogłoszenie (aby metoda GetUserId() była dostępna, 
	- zaimportuj bibliotekę using Microsoft.AspNet.Identity;):
	```
	ogloszenie.UzytkownikId = User.Identity.GetUserId(); 
	```

	- automatyczne przypisanie aktualnej daty jako DataDodania:
	```
	ogloszenie.DataDodania = DateTime.Now; 
	```
	
	- zabezpieczenie metody , by była dostępna tylko dla zalogowanych (jeśli użytkownik jest niezalogowany , nie ma możliwości przypisania id autora do ogłoszenia), poprzez użycie atrybutu:
	```
	[Authorize] 
	```

	- usunięcie kodu tworzącego listę użytkowników — usuń linię:
	```
	ViewBag.UzytkownikId = new SelectList(db.Users, "Id", "Email", ogloszenie.UzytkownikId); 
	```

	- w razie wystąpienia błędu powrót do widoku dodawania:
	```
	try {    
		_repo.SaveChanges();    
		return RedirectToAction("Index"); 
	} 
	catch {    
		return View(ogloszenie); 
	} 
	```

	- w bloku try przed zapisem wywołanie metody z repozytorium o nazwie `Dodaj()` służącej do dodania użytkownika, która zostanie utworzona w kolejnym kroku:
	```
	_repo.Dodaj(ogloszenie); 
	```
	
	- ustalenie potrzebnych parametrów dzięki Bind[Include=""]. Listing 8.10. 
	
	Kod po aktualizacji:
	```
	[Authorize]
	[HttpPost] 
	[ValidateAntiForgeryToken] 
	public ActionResult Create([Bind(Include = "Tresc,Tytul")] Ogloszenie ogloszenie) 
	{    
		if (ModelState.IsValid)    
		{        
			// using Microsoft.AspNet.Identity;        
			ogloszenie.UzytkownikId = User.Identity.GetUserId();        
			ogloszenie.DataDodania = DateTime.Now;        
			try        
			{            
				_repo.Dodaj(ogloszenie);            
				_repo.SaveChanges();            
				return RedirectToAction("Index");        
			}        
			catch        {            
				return View(ogloszenie);        
			}    
		}    
		return View(ogloszenie); 
	}
	```
	
* Zabezpieczenie metody przed atakami CSRF 
Aby zabezpieczyć witrynę przed CSRF , wykorzystaj atrybut [ValidateAntiForgeryToken]. W pliku z widokiem znajduje się kod: @Html.AntiForgeryToken() który jest umieszczony w treści formularza: @using (Html.BeginForm()) {    @Html.AntiForgeryToken()    ... } Podczas tworzenia widoku zostaje wygenerowany klucz, który jest przesyłany do przeglądarki w formie ukrytego pola `<input type="hidden">`: 
```
<input name="__RequestVerificationToken" type="hidden" value="jakiśKlucz" /> 
```

Następnie przeglądarka odsyła klucz w treści żądania POST (rysunek 8.88), po czym atrybut [ValidateAntiForgeryToken] sprawdza, czy przesłany klucz zgadza się z tym wysłanym podczas generowania widoku. Dla każdego żądania generowany jest inny klucz (token).

* Binding i walidacja żądania POST
Aby sprawdzić, czy przesłane dane pasują do modelu danych, wykorzystywany jest kod: 
```
if (ModelState.IsValid) 
```

Instrukcja if sprawdza, czy nie było błędów podczas tworzenia obiektu, na podstawie danych z żądania POST .
Aby określić, które dane będą zapisywane, wykorzystywany jest atrybut `[Bind(Include = "")]`. Domyślny kod wygląda następująco: 
```
public ActionResult Create([Bind(Include = "Id,Tresc,Tytul,DataDodania,UzytkownikId")] Ogloszenie ogloszenie)
```
Ponieważ w tym przypadku ważne są tylko Tresc i Tytul, pozostaw tylko te pola: 
```
public ActionResult Create([Bind(Include = "Tresc,Tytul")] Ogloszenie ogloszenie)
```

* Metoda Create() w repozytorium 
W interfejsie IOgloszenieRepo dodaj deklarację metody: 
```
void Dodaj(Ogloszenie ogloszenie); 
```

W repozytorium OgloszenieRepo dodaj metodę: 
```
public void Dodaj(Ogloszenie ogloszenie) 
{    
	_db.Ogloszenia.Add(ogloszenie); 
}
```

Dodać atrybut [Authorize] do metody GET w akcji Create: 
```
// GET: Ogloszenie/Create 
[Authorize] 
public ActionResult Create() 
{    
	return View(); 
} 
```

Walidacja danych po stronie klienta odbywa się za pomocą biblioteki jQuery , która jest ładowana w sekcji `Scripts` na końcu pliku z widokiem: 
```
@section Scripts {    
	@Scripts.Render("~/bundles/jqueryval") 
} 
```

Skrypt jest generowany za pomocą Boundling and Minification, a więc mechanizmu tworzenia paczek skryptów, który jest konfigurowany w pliku `BundleConfig` znajdującym się w folderze `App_Start`. Paczka `bundles/jqueryval` zawiera bibliotekę `jquery.validate` i jest skonfigurowana za pomocą:
```
bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.validate*"));
```

* Edit
```
public ActionResult Edit(int? id) 
{    
	if (id == null)    {        
		return new HttpStatusCodeResult(HttpStatusCode.BadRequest);    
	}    
	Ogloszenie ogloszenie = _repo.GetOgloszenieById((int)id);    
	if (ogloszenie == null)    {        
		return HttpNotFound();    
	}    
	return View(ogloszenie); 
} 
```

* Aktualizacja widoku dla akcji Edit 
	- Dodaj kolorowe przyciski i opisy do Google. 
	- Implementacja metody POST aby korzystała z obsługi błędów. 

* Implementacja metody POST
```
[HttpPost] 
[ValidateAntiForgeryToken] 
public ActionResult Edit([Bind(Include = "Id,Tresc,Tytul,DataDodania, UzytkownikId")] Ogloszenie ogloszenie)
{    
	if (ModelState.IsValid)    {        
		db.Entry(ogloszenie).State = EntityState.Modified;        
		db.SaveChanges();        
		return RedirectToAction("Index");    
	}    
	ViewBag.UzytkownikId = new SelectList(db.Users, "Id", "Email", ogloszenie.UzytkownikId);    
	return View(ogloszenie); 
}
```

