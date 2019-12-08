# TextDataBuilder
TextDataBuilder can build text file with datas like sql, csv, json, xml, ...

It's a .NET Core console program you can call with :
```
TextDataBuilder.exe <template file path>
```

Short example:
--
datas.csv :
```
Val1, Val2, Val3
Val4, Val5, Val6
```
This template :
```
This is a random number : @{RandomNumber As=RandVal Min=20 Max=100}
This is the same number : @{RandVal}

Transform CSV to SQL insert :

INSERT INTO MyTable
(Col1, Col2, Col3)
VALUES
@{CSV Path="data.csv"}
({0}, {1}, {2}),
@{/CSV}
```

Result :
```
This is a random number : 42
This is the same number : 42

Transform CSV to SQL insert :

INSERT INTO MyTable
(Col1, Col2, Col3)
VALUES
(Val1, Val2, Val3),
(Val4, Val5, Val6),
```
Template
--
The template define the generated text data. It is a combinaison of plain text and tag. The template is parsed by the application. 

Tag
---
The tags are replaced by the generated texts. It has the structure @{Prototype Parameter1=Value2, Parameter2=Value2}.

It begin with "@{" and end with "}". The first word is the prototype name, follow by the prototype parameters.

It has two type of tag. The Content tag and the data tag.


Data tag
---
Data tag are just replaced by text data.

|Prototype|Parameter|Required|Default|
|---------|---------|--------|-------|
|Text|Raw|Yes| |
|RandomInteger|Min|No|0|
| |Max|No|int.Max|

Content tag
---
Content tag has a start tag, a content and end tad. The start tag define the prototype. The end tag is a tag without parameters and the name is the start tag's name with the prefix '/'.

The content is between the start tag and the end tag. The content is parsed to print text.

For example, the CSV content tag will, for each line of the CSV file, print the content where `{<index>}` is replaced by the CSV column's value at this index.

|Prototype|Parameter|Required|Default|
|---------|---------|--------|-------|
|CSV|Path|Yes| |
