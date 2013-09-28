<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Faq and troubleshooting
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Faq and troubleshooting</h2>
These are common problems noticed so far:
<ul>
    <li>
        In Java's case it is mandatory to have <code>class Rextester</code> (note no access modifier) as an entry point for your code.
        There is simillar requirement in C#, but Java folks seem to miss this much more frequently.
    </li>
    <li>
        If your code waits for input from stdin (keyboard) you have to supply it all at once in the input text-area (which shows up after you click '+'
        sign at the bottom). The whole input should be supplied before running code - at runtime it will be piped to the stdin stream of the process.
    </li>
    <li>
        Codewall - put there code snippets you want to find later easily. For example, google for 'rebuild indexes rextester' to get Sql Server's script to rebuild all indexes.<br/>
        Personal code walls - put anything you like there. Nice and encouraged usage would be to put tutorials there as in <a href="http://rextester.com/users/31">C# tutorial</a>.
    </li>
    <li>
        If you have any questions - ask using <a href="https://groups.google.com/forum/#!forum/rextester">this google group.</a><br/><br/>.
    </li>
</ul>
<h2>Codemirror shortcuts</h2>
<ul>
    <li>Run snippet: 'F8' or 'F5' while cursor is in the editor.</li>
    <li>Full-screen mode: 'F11' while cursor is in the editor. 'Esc' to exit or 'F8' to exit and run.</li>
</ul>
<h2>Disquss comments</h2>
<ul>
    <li>Wrap code snippets in &lt;pre&gt;&lt/pre&gt; tags. All available tags are <a href="http://help.disqus.com/customer/portal/articles/466253-what-html-tags-are-allowed-within-comments-">here</a>.</li>
</ul>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MetaContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
