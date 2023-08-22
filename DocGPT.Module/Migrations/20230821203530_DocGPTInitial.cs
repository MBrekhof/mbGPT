using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Pgvector;

#nullable disable

namespace DocGPT.Module.Migrations
{
    /// <inheritdoc />
    public partial class DocGPTInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "article",
            //    columns: table => new
            //    {
            //        articleid = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        articlename = table.Column<string>(type: "character varying(250)", unicode: false, maxLength: 250, nullable: true),
            //        description = table.Column<string>(type: "character varying(250)", unicode: false, maxLength: 250, nullable: true),
            //        summary = table.Column<string>(type: "text", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("pk_article", x => x.articleid);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "chatmodel",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        name = table.Column<string>(type: "text", nullable: false),
            //        size = table.Column<int>(type: "integer", nullable: true),
            //        tokencost = table.Column<float>(type: "real", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("pk_chatmodel", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "codeobjectcategory",
            //    columns: table => new
            //    {
            //        codeobjectcategoryid = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        category = table.Column<string>(type: "text", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("pk_codeobjectcategory", x => x.codeobjectcategoryid);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "embeddingmodel",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        name = table.Column<string>(type: "text", nullable: true),
            //        size = table.Column<int>(type: "integer", nullable: true),
            //        tokencost = table.Column<float>(type: "real", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("pk_embeddingmodel", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "filedata",
            //    columns: table => new
            //    {
            //        id = table.Column<Guid>(type: "uuid", nullable: false),
            //        size = table.Column<int>(type: "integer", nullable: false),
            //        filename = table.Column<string>(type: "text", nullable: true),
            //        content = table.Column<byte[]>(type: "bytea", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("pk_filedata", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "filesystemstoreobjectbase",
            //    columns: table => new
            //    {
            //        id = table.Column<Guid>(type: "uuid", nullable: false),
            //        filename = table.Column<string>(type: "text", nullable: true),
            //        size = table.Column<int>(type: "integer", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("pk_filesystemstoreobjectbase", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "maildata",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        to = table.Column<List<string>>(type: "text[]", nullable: true),
            //        bcc = table.Column<List<string>>(type: "text[]", nullable: true),
            //        cc = table.Column<List<string>>(type: "text[]", nullable: true),
            //        from = table.Column<string>(type: "text", nullable: true),
            //        displayname = table.Column<string>(type: "text", nullable: true),
            //        replyto = table.Column<string>(type: "text", nullable: true),
            //        replytoname = table.Column<string>(type: "text", nullable: true),
            //        subject = table.Column<string>(type: "text", nullable: true),
            //        body = table.Column<string>(type: "text", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("pk_maildata", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "permissionpolicyrolebase",
            //    columns: table => new
            //    {
            //        id = table.Column<Guid>(type: "uuid", nullable: false),
            //        name = table.Column<string>(type: "text", nullable: true),
            //        isadministrative = table.Column<bool>(type: "boolean", nullable: false),
            //        caneditmodel = table.Column<bool>(type: "boolean", nullable: false),
            //        permissionpolicy = table.Column<int>(type: "integer", nullable: false),
            //        isallowpermissionpriority = table.Column<bool>(type: "boolean", nullable: false),
            //        discriminator = table.Column<string>(type: "text", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("pk_permissionpolicyrolebase", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "permissionpolicyuser",
            //    columns: table => new
            //    {
            //        id = table.Column<Guid>(type: "uuid", nullable: false),
            //        username = table.Column<string>(type: "text", nullable: true),
            //        isactive = table.Column<bool>(type: "boolean", nullable: false),
            //        changepasswordonfirstlogon = table.Column<bool>(type: "boolean", nullable: false),
            //        storedpassword = table.Column<string>(type: "text", nullable: true),
            //        discriminator = table.Column<string>(type: "text", nullable: false),
            //        email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
            //        phone = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("pk_permissionpolicyuser", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "prompt",
            //    columns: table => new
            //    {
            //        promptid = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        subject = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
            //        promptbody = table.Column<string>(type: "text", unicode: false, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("pk_prompt", x => x.promptid);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "similarcontentarticlesresult",
            //    columns: table => new
            //    {
            //        id = table.Column<long>(type: "bigint", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        articlename = table.Column<string>(type: "text", nullable: true),
            //        articlecontent = table.Column<string>(type: "text", nullable: true),
            //        articlesequence = table.Column<int>(type: "integer", nullable: true),
            //        cosine_distance = table.Column<double>(type: "double precision", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("pk_similarcontentarticlesresult", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "tag",
            //    columns: table => new
            //    {
            //        tagid = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        tagname = table.Column<string>(type: "text", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("pk_tag", x => x.tagid);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "websitedata",
            //    columns: table => new
            //    {
            //        websitedataid = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        website = table.Column<string>(type: "text", nullable: true),
            //        url = table.Column<string>(type: "text", nullable: true),
            //        text = table.Column<string>(type: "text", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("pk_websitedata", x => x.websitedataid);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "articledetail",
            //    columns: table => new
            //    {
            //        articledetailid = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        articleid = table.Column<int>(type: "integer", nullable: false),
            //        articlesequence = table.Column<int>(type: "integer", nullable: false),
            //        articlecontent = table.Column<string>(type: "text", nullable: true),
            //        tokens = table.Column<int>(type: "integer", nullable: false),
            //        vectordatastring = table.Column<Vector>(type: "vector(1536)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("pk_articledetail", x => x.articledetailid);
            //        table.ForeignKey(
            //            name: "fk_articledetail_article_articleid",
            //            column: x => x.articleid,
            //            principalTable: "article",
            //            principalColumn: "articleid");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "codeobject",
            //    columns: table => new
            //    {
            //        codeobjectid = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        subject = table.Column<string>(type: "text", nullable: true),
            //        codeobjectcategoryid = table.Column<int>(type: "integer", nullable: false),
            //        codeobjectcontent = table.Column<string>(type: "text", nullable: false),
            //        tokens = table.Column<int>(type: "integer", nullable: false),
            //        vectordatastring = table.Column<Vector>(type: "vector(1536)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("pk_codeobject", x => x.codeobjectid);
            //        table.ForeignKey(
            //            name: "fk_codeobject_codeobjectcategory_codeobjectcategoryid",
            //            column: x => x.codeobjectcategoryid,
            //            principalTable: "codeobjectcategory",
            //            principalColumn: "codeobjectcategoryid",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "settings",
            //    columns: table => new
            //    {
            //        settingsid = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        openaiorganization = table.Column<string>(type: "text", nullable: true),
            //        openaikey = table.Column<string>(type: "text", nullable: true),
            //        chatmodelid = table.Column<int>(type: "integer", nullable: true),
            //        embeddingmodelid = table.Column<int>(type: "integer", nullable: true),
            //        fromdisplayname = table.Column<string>(type: "text", nullable: true),
            //        fromemailname = table.Column<string>(type: "text", nullable: true),
            //        emailusername = table.Column<string>(type: "text", nullable: true),
            //        emailpassword = table.Column<string>(type: "text", nullable: true),
            //        smtphost = table.Column<string>(type: "text", nullable: true),
            //        smtpport = table.Column<int>(type: "integer", nullable: false),
            //        usessl = table.Column<bool>(type: "boolean", nullable: false),
            //        usestarttls = table.Column<bool>(type: "boolean", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("pk_settings", x => x.settingsid);
            //        table.ForeignKey(
            //            name: "fk_settings_chatmodel_chatmodelid",
            //            column: x => x.chatmodelid,
            //            principalTable: "chatmodel",
            //            principalColumn: "id");
            //        table.ForeignKey(
            //            name: "fk_settings_embeddingmodel_embeddingmodelid",
            //            column: x => x.embeddingmodelid,
            //            principalTable: "embeddingmodel",
            //            principalColumn: "id");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "filesystemstoreobject",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        fileid = table.Column<Guid>(type: "uuid", nullable: true),
            //        processed = table.Column<bool>(type: "boolean", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("pk_filesystemstoreobject", x => x.id);
            //        table.ForeignKey(
            //            name: "fk_filesystemstoreobject_filesystemstoreobjectbase_fileid",
            //            column: x => x.fileid,
            //            principalTable: "filesystemstoreobjectbase",
            //            principalColumn: "id");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "permissionpolicyactionpermissionobject",
            //    columns: table => new
            //    {
            //        id = table.Column<Guid>(type: "uuid", nullable: false),
            //        roleid = table.Column<Guid>(type: "uuid", nullable: true),
            //        actionid = table.Column<string>(type: "text", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("pk_permissionpolicyactionpermissionobject", x => x.id);
            //        table.ForeignKey(
            //            name: "fk_permissionpolicyactionpermissionobject_permissionpolicyrole~",
            //            column: x => x.roleid,
            //            principalTable: "permissionpolicyrolebase",
            //            principalColumn: "id");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "permissionpolicynavigationpermissionobject",
            //    columns: table => new
            //    {
            //        id = table.Column<Guid>(type: "uuid", nullable: false),
            //        roleid = table.Column<Guid>(type: "uuid", nullable: true),
            //        itempath = table.Column<string>(type: "text", nullable: true),
            //        targettypefullname = table.Column<string>(type: "text", nullable: true),
            //        navigatestate = table.Column<int>(type: "integer", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("pk_permissionpolicynavigationpermissionobject", x => x.id);
            //        table.ForeignKey(
            //            name: "fk_permissionpolicynavigationpermissionobject_permissionpolicy~",
            //            column: x => x.roleid,
            //            principalTable: "permissionpolicyrolebase",
            //            principalColumn: "id");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "permissionpolicytypepermissionobject",
            //    columns: table => new
            //    {
            //        id = table.Column<Guid>(type: "uuid", nullable: false),
            //        targettypefullname = table.Column<string>(type: "text", nullable: true),
            //        roleid = table.Column<Guid>(type: "uuid", nullable: true),
            //        readstate = table.Column<int>(type: "integer", nullable: true),
            //        writestate = table.Column<int>(type: "integer", nullable: true),
            //        createstate = table.Column<int>(type: "integer", nullable: true),
            //        deletestate = table.Column<int>(type: "integer", nullable: true),
            //        navigatestate = table.Column<int>(type: "integer", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("pk_permissionpolicytypepermissionobject", x => x.id);
            //        table.ForeignKey(
            //            name: "fk_permissionpolicytypepermissionobject_permissionpolicyroleba~",
            //            column: x => x.roleid,
            //            principalTable: "permissionpolicyrolebase",
            //            principalColumn: "id");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "permissionpolicyrolepermissionpolicyuser",
            //    columns: table => new
            //    {
            //        rolesid = table.Column<Guid>(type: "uuid", nullable: false),
            //        usersid = table.Column<Guid>(type: "uuid", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("pk_permissionpolicyrolepermissionpolicyuser", x => new { x.rolesid, x.usersid });
            //        table.ForeignKey(
            //            name: "fk_permissionpolicyrolepermissionpolicyuser_permissionpolicyro~",
            //            column: x => x.rolesid,
            //            principalTable: "permissionpolicyrolebase",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "fk_permissionpolicyrolepermissionpolicyuser_permissionpolicyus~",
            //            column: x => x.usersid,
            //            principalTable: "permissionpolicyuser",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "permissionpolicyuserlogininfo",
            //    columns: table => new
            //    {
            //        id = table.Column<Guid>(type: "uuid", nullable: false),
            //        loginprovidername = table.Column<string>(type: "text", nullable: true),
            //        provideruserkey = table.Column<string>(type: "text", nullable: true),
            //        userforeignkey = table.Column<Guid>(type: "uuid", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("pk_permissionpolicyuserlogininfo", x => x.id);
            //        table.ForeignKey(
            //            name: "fk_permissionpolicyuserlogininfo_permissionpolicyuser_userfore~",
            //            column: x => x.userforeignkey,
            //            principalTable: "permissionpolicyuser",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "chat",
            //    columns: table => new
            //    {
            //        chatid = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        question = table.Column<string>(type: "text", nullable: true),
            //        questiondatastring = table.Column<string>(type: "text", nullable: true),
            //        promptid = table.Column<int>(type: "integer", nullable: true),
            //        answer = table.Column<string>(type: "text", nullable: true),
            //        tokens = table.Column<int>(type: "integer", nullable: true),
            //        chatmodelid = table.Column<int>(type: "integer", nullable: true),
            //        created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("pk_chat", x => x.chatid);
            //        table.ForeignKey(
            //            name: "fk_chat_chatmodel_chatmodelid",
            //            column: x => x.chatmodelid,
            //            principalTable: "chatmodel",
            //            principalColumn: "id");
            //        table.ForeignKey(
            //            name: "fk_chat_prompt_promptid",
            //            column: x => x.promptid,
            //            principalTable: "prompt",
            //            principalColumn: "promptid");
            //    });

            migrationBuilder.CreateTable(
                name: "codeobjecttag",
                columns: table => new
                {
                    codeobjectscodeobjectid = table.Column<int>(type: "integer", nullable: false),
                    tagstagid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_codeobjecttag", x => new { x.codeobjectscodeobjectid, x.tagstagid });
                    table.ForeignKey(
                        name: "fk_codeobjecttag_codeobject_codeobjectscodeobjectid",
                        column: x => x.codeobjectscodeobjectid,
                        principalTable: "codeobject",
                        principalColumn: "codeobjectid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_codeobjecttag_tag_tagstagid",
                        column: x => x.tagstagid,
                        principalTable: "tag",
                        principalColumn: "tagid",
                        onDelete: ReferentialAction.Cascade);
                });

            //migrationBuilder.CreateTable(
            //    name: "permissionpolicymemberpermissionsobject",
            //    columns: table => new
            //    {
            //        id = table.Column<Guid>(type: "uuid", nullable: false),
            //        members = table.Column<string>(type: "text", nullable: true),
            //        criteria = table.Column<string>(type: "text", nullable: true),
            //        readstate = table.Column<int>(type: "integer", nullable: true),
            //        writestate = table.Column<int>(type: "integer", nullable: true),
            //        typepermissionobjectid = table.Column<Guid>(type: "uuid", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("pk_permissionpolicymemberpermissionsobject", x => x.id);
            //        table.ForeignKey(
            //            name: "fk_permissionpolicymemberpermissionsobject_permissionpolicytyp~",
            //            column: x => x.typepermissionobjectid,
            //            principalTable: "permissionpolicytypepermissionobject",
            //            principalColumn: "id");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "permissionpolicyobjectpermissionsobject",
            //    columns: table => new
            //    {
            //        id = table.Column<Guid>(type: "uuid", nullable: false),
            //        criteria = table.Column<string>(type: "text", nullable: true),
            //        readstate = table.Column<int>(type: "integer", nullable: true),
            //        writestate = table.Column<int>(type: "integer", nullable: true),
            //        deletestate = table.Column<int>(type: "integer", nullable: true),
            //        navigatestate = table.Column<int>(type: "integer", nullable: true),
            //        typepermissionobjectid = table.Column<Guid>(type: "uuid", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("pk_permissionpolicyobjectpermissionsobject", x => x.id);
            //        table.ForeignKey(
            //            name: "fk_permissionpolicyobjectpermissionsobject_permissionpolicytyp~",
            //            column: x => x.typepermissionobjectid,
            //            principalTable: "permissionpolicytypepermissionobject",
            //            principalColumn: "id");
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "ix_articledetail_articleid",
            //    table: "articledetail",
            //    column: "articleid");

            //migrationBuilder.CreateIndex(
            //    name: "ix_chat_chatmodelid",
            //    table: "chat",
            //    column: "chatmodelid");

            //migrationBuilder.CreateIndex(
            //    name: "ix_chat_promptid",
            //    table: "chat",
            //    column: "promptid");

            //migrationBuilder.CreateIndex(
            //    name: "ix_codeobject_codeobjectcategoryid",
            //    table: "codeobject",
            //    column: "codeobjectcategoryid");

            //migrationBuilder.CreateIndex(
            //    name: "ix_codeobjecttag_tagstagid",
            //    table: "codeobjecttag",
            //    column: "tagstagid");

            //migrationBuilder.CreateIndex(
            //    name: "ix_filesystemstoreobject_fileid",
            //    table: "filesystemstoreobject",
            //    column: "fileid");

            //migrationBuilder.CreateIndex(
            //    name: "ix_permissionpolicyactionpermissionobject_roleid",
            //    table: "permissionpolicyactionpermissionobject",
            //    column: "roleid");

            //migrationBuilder.CreateIndex(
            //    name: "ix_permissionpolicymemberpermissionsobject_typepermissionobjec~",
            //    table: "permissionpolicymemberpermissionsobject",
            //    column: "typepermissionobjectid");

            //migrationBuilder.CreateIndex(
            //    name: "ix_permissionpolicynavigationpermissionobject_roleid",
            //    table: "permissionpolicynavigationpermissionobject",
            //    column: "roleid");

            //migrationBuilder.CreateIndex(
            //    name: "ix_permissionpolicyobjectpermissionsobject_typepermissionobjec~",
            //    table: "permissionpolicyobjectpermissionsobject",
            //    column: "typepermissionobjectid");

            //migrationBuilder.CreateIndex(
            //    name: "ix_permissionpolicyrolepermissionpolicyuser_usersid",
            //    table: "permissionpolicyrolepermissionpolicyuser",
            //    column: "usersid");

            //migrationBuilder.CreateIndex(
            //    name: "ix_permissionpolicytypepermissionobject_roleid",
            //    table: "permissionpolicytypepermissionobject",
            //    column: "roleid");

            //migrationBuilder.CreateIndex(
            //    name: "ix_permissionpolicyuserlogininfo_userforeignkey",
            //    table: "permissionpolicyuserlogininfo",
            //    column: "userforeignkey");

            //migrationBuilder.CreateIndex(
            //    name: "ix_settings_chatmodelid",
            //    table: "settings",
            //    column: "chatmodelid");

            //migrationBuilder.CreateIndex(
            //    name: "ix_settings_embeddingmodelid",
            //    table: "settings",
            //    column: "embeddingmodelid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "articledetail");

            migrationBuilder.DropTable(
                name: "chat");

            migrationBuilder.DropTable(
                name: "codeobjecttag");

            migrationBuilder.DropTable(
                name: "filedata");

            migrationBuilder.DropTable(
                name: "filesystemstoreobject");

            migrationBuilder.DropTable(
                name: "maildata");

            migrationBuilder.DropTable(
                name: "permissionpolicyactionpermissionobject");

            migrationBuilder.DropTable(
                name: "permissionpolicymemberpermissionsobject");

            migrationBuilder.DropTable(
                name: "permissionpolicynavigationpermissionobject");

            migrationBuilder.DropTable(
                name: "permissionpolicyobjectpermissionsobject");

            migrationBuilder.DropTable(
                name: "permissionpolicyrolepermissionpolicyuser");

            migrationBuilder.DropTable(
                name: "permissionpolicyuserlogininfo");

            migrationBuilder.DropTable(
                name: "settings");

            migrationBuilder.DropTable(
                name: "similarcontentarticlesresult");

            migrationBuilder.DropTable(
                name: "websitedata");

            migrationBuilder.DropTable(
                name: "article");

            migrationBuilder.DropTable(
                name: "prompt");

            migrationBuilder.DropTable(
                name: "codeobject");

            migrationBuilder.DropTable(
                name: "tag");

            migrationBuilder.DropTable(
                name: "filesystemstoreobjectbase");

            migrationBuilder.DropTable(
                name: "permissionpolicytypepermissionobject");

            migrationBuilder.DropTable(
                name: "permissionpolicyuser");

            migrationBuilder.DropTable(
                name: "chatmodel");

            migrationBuilder.DropTable(
                name: "embeddingmodel");

            migrationBuilder.DropTable(
                name: "codeobjectcategory");

            migrationBuilder.DropTable(
                name: "permissionpolicyrolebase");
        }
    }
}
