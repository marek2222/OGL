# OGL
Na podstawie książki i dodatku K. Żydzika: C# 6.0 i MVC 5.0 Tworzenie nowoczesnych portali internetowych

Wzorce projektowe i architektoniczne wykorzystywane w .NET 
- Repozytorium - operacje korzystające z bazy danych są przenoszone do innej warstwy w aplikacji (osobnego projektu). W kontrolerze wywołuje się tylko metody z repozytorium. Z kilku kontrolerów można wywoływać tę samą metodę bez konieczności kopiowania lub ponownego pisania. Kontroler nie odpowiada za utworzenie kontekstu (obiektu dostępu do danych) i nie posiada informacji na temat tego, skąd są dane ani jaką strukturę ma baza danych. 
- Wzorzec IoC - Repozytorium ma jeden duży minus — instancja repozytorium jest tworzona w kontrolerze, co powoduje, że każdy kontroler jest zależny nie od kontekstu, a od repozytorium. Aby tworzenie obiektu repozytorium było niezależne od kontrolera, wykorzystuje się wzorzec IoC (ang. Inversion of Control). Dzięki takiemu rozwiązaniu instancja repozytorium jest wstrzykiwana poprzez konstruktor. IoC pozwala na działanie na interfejsie zamiast na określonej klasie. Dzięki temu można w łatwy sposób podmienić implementację repozytorium. W konfiguracji biblioteki odpowiedzialnej za IoC wybiera się, jaka klasa ma być wstrzyknięta dla danego interfejsu. Może być na przykład kilka wersji tego samego repozytorium. 
- Repozytorium generyczne - Bez względu na rodzaj klasy , na której się działa, istnieje kilka podstawowych metod wykorzystywanych w przypadku większości klas. Takimi metodami są na przykład operacje dodawania lub usuwania. Aby nie pisać tych samych deklaracji metod w każdym repozytorium, należy utworzyć generyczny interfejs repozytorium, po którym będą dziedziczyć wszystkie pozostałe interfejsy lub repozytoria. Dzięki repozytorium generycznemu nie ma potrzeby przepisywania tych samych metod do różnych interfejsów lub klas. Według niektórych wzorzec ten jest uznawany za antywzorzec. 
- Wzorzec UnitOfWork - pozwala na grupowanie kilku zadań z różnych repozytoriów w jedną transakcję. Służy do dzielenia pojedynczego kontekstu pomiędzy różne repozytoria. UnitOfWork znajduje zastosowanie, gdy chce się wprowadzić zmiany w wielu tabelach za pomocą wielu repozytoriów na pojedynczym kontekście. W przypadku bardzo wielu pojedynczych, prostych zapytań do bazy danych z różnych repozytoriów tworzone są osobne konteksty , a każde zapytanie jest wysyłane jako pojedyncze żądanie. Korzystając z UnitOfWork, oszczędza się zasoby serwera, ponieważ nie trzeba tworzyć wielu osobnych połączeń z bazą ani wielu kontekstów. Wszystkie operacje korzystają z pojedynczego kontekstu i pojedynczego połączenia.

Etap 1. Krok 1. Tworzenie nowego projektu i aktualizacja pakietów 
- nazwa projektu i solucji: OGL, wybrany template: MVC, referencje do MVC i WebApi, zaznaczono Add Unit Tests: Ogl.Tests, pooświadczenie: Indywidual User Account
- aktualizacja bibliotek Manage NuGet Packages i restart aplikacji

Etap 1. Krok 2. Utworzenie modelu danych w podejściu Code First
- folder Models mam utworzony 
- dodanie do niego klasy Ogloszenie, 
- wypełnienie ciała klasy: 
	- relacja 1-* (wiele ogłoszeń do jednego użytkownika) 
	```
	public string UzytkownikId { get; set; }
	public virtual Uzytkownik Uzytkownik { get; set; }
	```
	- oraz *-* (wiele ogłoszeń - wiele kategorii: wymaga dodatkowej klasa Ogloszenie_Kategoria dla której mamy relację 1-* )
	```
	public virtual ICollection<Ogloszenie_Kategoria>Ogloszenie_Kategoria { get; set; }	
	```
	- dodanie referencji: using System.ComponentModel.DataAnnotations;
- dodanie klasy Kategoria:
	- relacja *-* (wiele ogłoszeń - wiele kategorii: do klasy Ogloszenie_Kategoria mamy relację 1-* )
	```
	public Kategoria()    {   this.Ogloszenie_Kategoria = new HashSet<Ogloszenie_Kategoria>();    }
	public ICollection<Ogloszenie_Kategoria> Ogloszenie_Kategoria { get; set; }
	```
- dodanie klasy Ogloszenie_Kategoria
```
public Ogloszenie_Kategoria()    {    }    
public int KategoriaId {get; set;}    
public int OgloszenieId { get; set; }    
public virtual Kategoria Kategoria { get; set; }    
public virtual Ogloszenie Ogloszenie {get; set;} 
```
- klasa Uzytkownik
	- zmiana nazwy klasy ApplicationUser na Uzytkownik.
	- relacja 1-* (jednego użytkownika do wielu ogłoszeń) 
	```
	public Uzytkownik()    {        this.Ogloszenia = new HashSet<Ogloszenie>();    } 
	public virtual ICollection<Ogloszenie> Ogloszenia { get; private set; } 
	```

Etap 1. Krok 3. Tworzenie klasy kontekstu 
- w folderze Models zmieniamy nazwę pliku z IdentityModels na OglContext. Po zmianie nazwy pliku otwórz plik kontekstu (teraz już o nazwie OglContext) i zmień nazwę klasy z ApplicationDbContext na OglContext (razem z konstruktorem i statyczną metodą) 
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
- Teraz w całej aplikacji trzeba zamienić ApplicationDbContext na OglContext
- Jeśli nie chcesz korzystać z gotowej funkcjonalności lub nie musisz uwierzytelniać użytkowników, możesz utworzyć 	klasę dziedziczącą po DbContext: 	
```
public class OglContext : DbContext {}  
```
Korzystamy tu z gotowej funkcjonalności lub chcę uwierzytelniać użytkowników, więc nie korzystam z DbContext.
- Teraz dodaj do niej właściwości DbSet reprezentujące w pamięci kontekstu tabele z bazy danych. Oto kod:
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
-  Aby aplikacja nie zmieniała nazw, trzeba nadpisać konwencję. Jeśli chcesz to zrobić dla kontekstu, skorzystaj z metody: 
```
modelBuilder.Conventions.Remove<PluralizingTableNameConvention>(); 
```
Następnie dla relacji Ogloszenie – Uzytkownik włączono CascadeDelete za pomocą Fluent API. 	Oto kod:
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
-  W folderze Models znajdują się jeszcze pliki: AccountViewModels i ManageViewModels. Są to klasy z ViewModel na potrzeby logowania przez zewnętrzne serwisy i do zarządzania kontem użytkownika. Należy utworzyć folder Views w folderze Models i przenieść tam obydwa pliki. 

Etap 1. Krok 4. Przenoszenie warstwy modelu do osobnego projektu 		
- Utwórz mowy projekt ASP .NET Web Application i nazwij projekt Repozytorium. Wybierz template: Empty
- Dodawanie referencji pomiędzy projektami:  Dodaj referencję do Repozytorium z projektu OGL, kliknij prawym przyciskiem myszy References w projekcie OGL (rysunek 8.24) i wybierz Add Reference: Repozytorium. 
- Ustawienie projektu startowego na OGL
- Instalacja bibliotek dla projektu Repozytorium: Entity Framework, Microsoft ASP.NET MVC, Microsoft ASP.NET Identity Framework 
- Przenoszenie plików z modelem do osobnej warstwy (projektu) z OGL do Repozytorium, poza ManageViewModels.cs ponieważ posiada on referencje do Microsoft.Owin.Security, a nie chcemy instalować pakietu OWIN w projekcie Repozytorium. 
- Zmień nazwy w aplikacji z: OGL.Models na: Repozytorium.Models
- i Rebuild Solution
	
Etap 1. Krok 5. Migracje
- Instalacja migracji: W oknie Package Manager Console wybieramy projekt Repozytorium i wpisujemy Enable-Migration. Zostanie utworzony folder Migrations, a w nim plik konfiguracyjny Configuration
- Konfiguracja migracji: Aby włączyć automatyczne migracje oraz włączyć migracje stratne, dodaj następujący kod w konstruktorze klasy konfiguracyjnej: 
```
public Configuration() {    
	AutomaticMigrationsEnabled = true;    
	AutomaticMigrationDataLossAllowed = true; 
} 
	```
Od teraz bez pytania po zmianach w klasach z modelem będą wykonywane również migracje stratne, po czym zostanie wykonana metoda Seed. 
- Tworzenie migracji początkowej: 
	```
	Add-migration startowa 
	```
- Uruchomienie pierwszej migracji:
	```
	Update-Database
	```
- W Web.config zmianiam nazwę do bazy danych z pliku .mdf na bazę Sql Server następująco: 
	```
	connectionString="Data Source= (LocalDb)\v11.0;AttachDbFilename=|DataDirectory|\aspnet-OGL20140807102454.mdf;Initial Catalog=aspnet-OGL-20140807102454;Integrated Security=True" 
	```
- Ponownie uruchomiemy migrację:
	```
	Update-Database
	```
- Uzupelniam metodę Seed()
- Debugowanie metody Seed: tylko jeśli dodamy wpis na początku metody Seed(). Po debugowaniu zakomentuj wpis:
	```
	if (System.Diagnostics.Debugger.IsAttached == false)    
		System.Diagnostics.Debugger.Launch(); 
	```
- Zmiany w modelu i kolejna migracja: 
	Dodaj pole Wiek do klasy Uzytkownik: 
	```
	public int Wiek { get; set; } 
	```
	Zapisz plik i uruchom komendę: 
	```
	Add-migration 1 
	```
	gdzie 1 to nazwa migracji. 
	- Aby było możliwe przypisanie wartości null, pole Wiek należy oznaczyć jako Nullable i uruchomić migrację
	```
	public int? Wiek { get; set; } 
	```
- W SeedUsers() zmień kod 
	```
	var user = new Uzytkownik { UserName = "Admin" }; 
	```
	należy zastąpić następującym (aby przepisać wiek użytkownikowi): 
	```
	var user = new Uzytkownik { UserName = "Admin", Wiek = 12 }; 
	```
	

Etap 2. Krok 1. Dodawanie kontrolerów i widoków — akcja Index
- Dodawanie kontrolera z widokami przez: MVC 5 Controller with views, using Entity Framework
		Model: Ogloszenie, Context: OglContext, Nazwa: OgloszenieControlle.
		Zaznacz: Generate Views, Reference script libraries i Use a layout page
- Lista ogłoszeń (akcja Index) — aktualizacja widoku/wyglądu strony 
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

	
Etap 2. Krok 2. Debugowanie oraz metody AsNoTracking() i ToList()	

Etap 2. Krok 3. Poprawa wyglądu i optymalizacja pod kątem SEO 
- Poprawa wyglądu strony za pomocą Twitter Bootstrap (Ogloszenie/Index)
	```
	@Html.ActionLink("Dodaj nowe ogłoszenie", "Create") na: 
	@Html.ActionLink("Dodaj nowe ogłoszenie", "Create", null, new { @class = "btn btn-primary" }) 
	@Html.ActionLink("Szczegóły", "Details", new { id = item.Id }, new { @class = "btn btn-warning" })    <br />         
	@Html.ActionLink("Edytuj ", "Edit", new { id = item.Id },new { @class = "btn btn-primary" })    <br />    
	@Html.ActionLink("Usuń", "Delete", new { id = item.Id }, new { @class = "btn btndanger" })
	```
- Podświetlanie wierszy za pomocą CSS: Site.css: 
	```
	tr:first-child{    background-color:#efefef; }
	tr:hover td{    background-color:#efefef; } 
	```
		oraz dodaj odstępy pomiędzy przyciskami: 
	```
	.btn {    margin:2px; } 
	```
- Optymalizacja pod kątem pozycjonowania — SEO: tytuł, metaopis i słowa kluczowe
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


Etap 3. Krok 1. Poprawa architektury aplikacji
- Przeniesienie zapytania LINQ do osobnej metody
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
- Przeniesienie metody do repozytorium
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
- Wstrzykiwanie repozytorium poprzez konstruktor w kontrolerze
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
- Tworzenie interfejsu dla repozytorium
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
	- Instalacja kontenera IoC Unity: Unity.Mvc 
	