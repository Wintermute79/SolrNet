using NUnit.Framework;
using Rhino.Mocks;
using SolrNet.Commands;
using SolrNet.Commands.Parameters;

namespace SolrNet.Tests {
	[TestFixture]
	public class DeleteCommandTests {
		[Test]
		public void DeleteById() {
			const string id = "123123";
			MockRepository mocks = new MockRepository();
			ISolrConnection conn = mocks.CreateMock<ISolrConnection>();
			With.Mocks(mocks).Expecting(delegate {
				Expect.Call(conn.Post("/update", string.Format("<delete><id>{0}</id></delete>", id))).Repeat.Once().Return("");
			}).Verify(delegate {
				DeleteCommand cmd = new DeleteCommand(new DeleteByIdParam(id));
				cmd.Execute(conn);
			});
		}

		[Test]
		public void DeleteByQuery() {
			MockRepository mocks = new MockRepository();
			ISolrConnection conn = mocks.CreateMock<ISolrConnection>();
			ISolrQuery<ISolrDocument> q = mocks.CreateMock<ISolrQuery<ISolrDocument>>();
			With.Mocks(mocks).Expecting(delegate {
				const string queryString = "someQuery";
				Expect.Call(q.Query).Repeat.Once().Return(queryString);
				Expect.Call(conn.Post("/update", string.Format("<delete><query>{0}</query></delete>", queryString))).Repeat.Once().
					Return("");
			}).Verify(delegate {
				DeleteCommand cmd = new DeleteCommand(new DeleteByQueryParam<ISolrDocument>(q));
				cmd.Execute(conn);
			});
		}

		[Test]
		public void DeleteFromCommitted() {
			const string id = "123123";
			MockRepository mocks = new MockRepository();
			ISolrConnection conn = mocks.CreateMock<ISolrConnection>();
			With.Mocks(mocks).Expecting(delegate {
				Expect.Call(conn.Post("/update", string.Format("<delete fromCommitted=\"true\"><id>{0}</id></delete>", id))).Repeat.Once().Return("");
			}).Verify(delegate {
				DeleteCommand cmd = new DeleteCommand(new DeleteByIdParam(id));
				cmd.FromCommitted = true;
				cmd.Execute(conn);
			});
		}

		[Test]
		public void DeleteFromCommittedAndFromPending() {
			const string id = "123123";
			MockRepository mocks = new MockRepository();
			ISolrConnection conn = mocks.CreateMock<ISolrConnection>();
			With.Mocks(mocks).Expecting(delegate {
				Expect.Call(conn.Post("/update", string.Format("<delete fromPending=\"false\" fromCommitted=\"false\"><id>{0}</id></delete>", id))).Repeat.Once().Return("");
			}).Verify(delegate {
				DeleteCommand cmd = new DeleteCommand(new DeleteByIdParam(id));
				cmd.FromCommitted = false;
				cmd.FromPending = false;
				cmd.Execute(conn);
			});
		}

		[Test]
		public void DeleteFromPending() {
			const string id = "123123";
			MockRepository mocks = new MockRepository();
			ISolrConnection conn = mocks.CreateMock<ISolrConnection>();
			With.Mocks(mocks).Expecting(delegate {
				Expect.Call(conn.Post("/update", string.Format("<delete fromPending=\"true\"><id>{0}</id></delete>", id))).Repeat.Once().Return("");
			}).Verify(delegate {
				DeleteCommand cmd = new DeleteCommand(new DeleteByIdParam(id));
				cmd.FromPending = true;
				cmd.Execute(conn);
			});
		}
	}
}