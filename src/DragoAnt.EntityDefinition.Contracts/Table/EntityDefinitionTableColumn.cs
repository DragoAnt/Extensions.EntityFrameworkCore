﻿using System.Diagnostics;

namespace DragoAnt.EntityDefinition.Contracts.Table;

[DebuggerDisplay("Info:{Info.Name}, Type:{ColumnType}, Name:{ColumnName}")]
public record EntityDefinitionTableColumn(DefinitionInfo Info, string ColumnName,
    DefinitionColumnType ColumnType, Func<object?, string?> ConvertToStringFunc);