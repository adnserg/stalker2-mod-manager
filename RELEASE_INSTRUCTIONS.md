# Инструкция по созданию релиза на GitHub

## Автоматический способ (через скрипт)

### 1. Создайте GitHub Personal Access Token

1. Перейдите на https://github.com/settings/tokens
2. Нажмите "Generate new token" → "Generate new token (classic)"
3. Укажите название токена (например, "Release Token")
4. Выберите scope `repo` (полный доступ к репозиториям)
5. Нажмите "Generate token"
6. Скопируйте токен (он будет показан только один раз!)

### 2. Запустите скрипт

```powershell
.\create_release.ps1 -Token "ваш_github_token"
```

Скрипт автоматически:
- Создаст release на GitHub
- Загрузит архив `Stalker2ModManager-v1.0.0.zip`

## Ручной способ (через веб-интерфейс GitHub)

### 1. Перейдите на страницу релизов

Откройте: https://github.com/adnserg/stalker2-mod-manager/releases/new

### 2. Заполните форму

- **Choose a tag**: Выберите `v1.0.0` или создайте новый тег
- **Release title**: `Release v1.0.0` или `v1.0.0 - First Stable Release`
- **Description**: Скопируйте содержимое из файла `RELEASE_NOTES.md`

### 3. Загрузите архив

- Нажмите "Attach binaries by dropping them here or selecting them"
- Выберите файл `Stalker2ModManager-v1.0.0.zip`

### 4. Опубликуйте релиз

- Нажмите "Publish release"

## Что уже сделано

✅ Версия установлена в проект (1.0.0)  
✅ Тег `v1.0.0` создан и запушен на GitHub  
✅ Приложение собрано в Release конфигурации  
✅ Архив `Stalker2ModManager-v1.0.0.zip` создан  
✅ Release notes подготовлены (`RELEASE_NOTES.md`)

## Текущий статус

- Тег: `v1.0.0` - создан и запушен
- Архив: `Stalker2ModManager-v1.0.0.zip` - создан
- Release: требуется создать через GitHub API или веб-интерфейс

