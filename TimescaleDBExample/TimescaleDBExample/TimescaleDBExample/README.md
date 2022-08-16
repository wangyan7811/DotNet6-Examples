## 建表
```
CREATE TABLE tag_history_value (
  time TIMESTAMPTZ NOT NULL,
  TagName TEXT NOT NULL,
  Val DOUBLE PRECISION NULL,
  Quality INT NULL
);
```
## 转换超级表
```
SELECT create_hypertable('tag_history_value','time');
```
