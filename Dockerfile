# ���������� ����������� ����� .NET SDK ��� ������
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# ������������� ������� ���������� ������ ����������
WORKDIR /app

# �������� ��� ����� ������� � ���������
COPY . .

# ��������������� ����������� �������
RUN dotnet restore

# ������ ������
RUN dotnet publish -c Release -o /app/publish

# ���������� ����� ������ ����� ��� ������� (���������� ������ runtime, � �� SDK)
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# ������������� ������� ���������� ��� �������
WORKDIR /app

# �������� ��������� ������ �� ���������� "build"
COPY --from=build /app/publish .

# ��������� ������� ��� ������ ����������
ENTRYPOINT ["dotnet", "YourProjectName.dll"]
