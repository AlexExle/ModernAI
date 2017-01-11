// ���� ���� ���������� ��������� ������ � ���������, ������� �� ����� ������������
// � �������� �� ��������� ��������

open System
Environment.CurrentDirectory <- __SOURCE_DIRECTORY__

let random = new Random()

// ���������� ���������� ������������� ��������� ���������� �����������
let rnd n = 
 {1..10} 
 |> Seq.map (fun _ -> random.Next(-n,n))
 |> Seq.sum


#r "FSharp.Data.dll"
open FSharp.Data

// ��������� ���� � ������� � ����������� �������� �� ����� ������
type TData = CsvProvider<"Skate.csv">
let data = TData.Load("Skate.csv")

#r "System.Device.dll"
open System.Device.Location

// ���������� �� ������ ������ � ���������� �� �����������
let center = new GeoCoordinate(55.751999,37.617734)
let dist lat long = center.GetDistanceTo(new GeoCoordinate(lat,long)) / 1000.
   
let sb = new System.Text.StringBuilder()

sprintf "Rooms,DistCenter,Area,Price,AdmArea,District,Lat,Long,Address" |> sb.AppendLine

for x in data.Rows do
    let d = dist (x.Lat|>float) (x.Long|>float)
    let r = random.Next(1,4)
    let a = (20+random.Next(-5,5))*r+random.Next(6,12)
    let p = a*(170+rnd 10)+70*(20-int(d))+rnd 20
    if (d<20.) then sprintf "%d,%f,%d,%d,%s,%s,%f,%f,\"%s\"" r d a p x.AdmArea x.District x.Lat x.Long x.Address |> sb.AppendLine |> ignore

System.IO.File.AppendAllText(@"ApartmentData.csv",sb.ToString())
