﻿using System;
using NPoco;
using Umbraco.Core.Persistence.DatabaseAnnotations;
using Umbraco.Core.Persistence.DatabaseModelDefinitions;

namespace Umbraco.Core.Models.Rdbms
{
    [TableName(TableName)]
    [PrimaryKey("id")]
    [ExplicitColumns]
    internal class ContentVersionDto
    {
        private const string TableName = Constants.DatabaseSchema.Tables.ContentVersion;

        [Column("id")]
        [PrimaryKeyColumn]
        public int Id { get; set; }

        [Column("nodeId")]
        [ForeignKey(typeof(ContentDto))]
        [Index(IndexTypes.UniqueNonClustered, Name = "IX_" + TableName + "_NodeIdVersionId", ForColumns = "nodeId, versionId")]
        public int NodeId { get; set; }

        [Column("versionId")]
        [Index(IndexTypes.UniqueNonClustered)]
        public Guid VersionId { get; set; }

        [Column("versionDate")]
        [Constraint(Default = SystemMethods.CurrentDateTime)]
        public DateTime VersionDate { get; set; }

        [Column("userId")]
        public int UserId { get; set; }

        [Column("current")]
        public bool Current { get; set; }

        // about current:
        // there is nothing in the DB that guarantees that there will be one, and exactly one, current version per content item.
        // that would require circular FKs that are impossible (well, it is possible to create them, but not to insert).
        // we could use a content.currentVersionId FK that would need to be nullable, or (better?) an additional table
        // linking a content itemt to its current version (nodeId, versionId) - that would guarantee uniqueness BUT it would
        // not guarantee existence - so, really... we are trusting our code to manage 'current' correctly.

        [Column("text")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public string Text { get; set; }

        [ResultColumn]
        [Reference(ReferenceType.OneToOne, ColumnName = "NodeId", ReferenceMemberName = "NodeId")]
        public ContentDto ContentDto { get; set; }
    }
}
