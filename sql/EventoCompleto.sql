﻿SELECT * FROM EVENTOS E
INNER JOIN ENDERECOS EN ON E.ID = EN.EVENTOID
INNER JOIN CATEGORIAS C ON E.CATEGORIAID = C.ID
INNER JOIN ORGANIZADORES O ON O.ID = E.ORGANIZADORID