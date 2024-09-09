CREATE TABLE Farmers (
    FarmerID INT IDENTITY(1,1) PRIMARY KEY,
    ContactInfo NVARCHAR(255),
    Address NVARCHAR(255)
);

CREATE TABLE UserCredentials (
    FarmerID INT PRIMARY KEY,
    FirstName NVARCHAR(100),
    LastName NVARCHAR(100),
    Password NVARCHAR(255),
    FOREIGN KEY (FarmerID) REFERENCES Farmers(FarmerID)
);

CREATE TABLE Animals (
    AnimalID INT IDENTITY(1,1) PRIMARY KEY,
    Species NVARCHAR(100),
    VaccinationStatus NVARCHAR(255),
    FarmerID INT,
    FOREIGN KEY (FarmerID) REFERENCES Farmers(FarmerID)
);

CREATE TABLE Crops (
    CropID INT IDENTITY(1,1) PRIMARY KEY,
    Type NVARCHAR(100),
    PlantingDate DATE,
    HarvestDate DATE,
    FarmerID INT,
    FOREIGN KEY (FarmerID) REFERENCES Farmers(FarmerID)
);

CREATE TABLE Weather (
    WeatherID INT IDENTITY(1,1) PRIMARY KEY,
    Date DATE,
    Temperature DECIMAL(5,2),
    Humidity DECIMAL(5,2)
);

CREATE TABLE AnimalStocks (
    StockID INT IDENTITY(1,1) PRIMARY KEY,
    FarmerID INT,
    AnimalID INT,
    Quantity DECIMAL(10,2),
    FOREIGN KEY (FarmerID) REFERENCES Farmers(FarmerID),
    FOREIGN KEY (AnimalID) REFERENCES Animals(AnimalID)
);

CREATE TABLE CropStocks (
    StockID INT IDENTITY(1,1) PRIMARY KEY,
    FarmerID INT,
    CropID INT,
    Quantity DECIMAL(10,2),
    FOREIGN KEY (FarmerID) REFERENCES Farmers(FarmerID),
    FOREIGN KEY (CropID) REFERENCES Crops(CropID)
);

CREATE TABLE CropMarket (
    MarketID INT IDENTITY(1,1) PRIMARY KEY,
    CropID INT,
    Price DECIMAL(10,2),
    FOREIGN KEY (CropID) REFERENCES Crops(CropID)
);

CREATE TABLE AnimalMarket (
    MarketID INT IDENTITY(1,1) PRIMARY KEY,
    AnimalID INT,
    Price DECIMAL(10,2),
    FOREIGN KEY (AnimalID) REFERENCES Animals(AnimalID)
);
